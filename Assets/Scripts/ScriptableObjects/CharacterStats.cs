using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Declarations and Definitions

[System.Serializable]
public class Stats
{
    #region Actual Stats

    [SerializeField]
    private float health = 0.0f;

    [SerializeField]
    private float damage = 0.0f;

    [SerializeField]
    private float endurance = 0.0f;

    [SerializeField]
    private int experience = 0;

    [SerializeField]
    private int level = 1;

    #endregion Actual Stats

    #region Buffer Calculation Variables

    private int _ExpBase = 10;
    private int _ExpLeft = 10;
    private float _ExpMod = 1.11f;
    private float _t = 0.0f;

    #endregion Buffer Calculation Variables

    #region Public Methods

    public float AlterHealthValue(float value = 0.0f)
    {
        if (value != 0)
            health += value;
        return health;
    }

    public float AlterDamageValue(float value = 0.0f)
    {
        if (value != 0)
            damage += value;
        return damage;
    }

    public float AlterEnduranceValue(float value = 0.0f)
    {
        if (value != 0)
            endurance += value;
        return endurance;
    }

    public int AlterExperienceValue(int value = 0)
    {
        if (value != 0)
        {
            experience += value;
            if (experience >= _ExpLeft)
            {
                experience -= _ExpLeft;
                level++;
                _t = Mathf.Pow(_ExpMod, level);
                _ExpLeft = Mathf.FloorToInt(_ExpBase * _t);
            }
        }
        return level;
    }

    #endregion Public Methods
}

[System.Serializable]
public class SkillData
{
    [System.Serializable]
    public enum SkillType
    {
        Passive = 0,
        Ability = 1,
    }

    [SerializeField]
    public enum PassiveStatModifier
    {
        Ability = 0,
        Health = 1,
        Damage = 2,
        Endurance = 3,
    }

    public int skillID; //Unique Identifier Value for skill

    public string skillName;

    [TextArea]
    public string skillDescription;

    public int minimumLevel = 1;
    public List<int> skillPrerequisites; //List of unique Skill IDs which need to be unlocked before unlocking current skill
    public bool unlocked = false;
    public SkillType skillType;
    public PassiveStatModifier passiveStatMultiplier;
    public float multiplierValue;
}

[System.Serializable]
public class SkillTree
{
    #region Buffer Calculation Variables

    private SkillData _bufferSkill = null;

    #endregion Buffer Calculation Variables

    private List<SkillData> availableSkillList = null;
    public List<SkillData> skillTree;

    public bool CheckIfAvailable(SkillData skillNode)
    {
        if (skillNode.unlocked)
        {
            return false;
        }
        if (skillNode.skillPrerequisites.Count == 0)
        {
            return true;
        }
        for (int i = 0; i < skillNode.skillPrerequisites.Count; i++)
        {
            for (int j = 0; j < skillTree.Count; j++)
            {
                if (skillNode.skillPrerequisites[i] == skillTree[j].skillID)
                {
                    if (!skillTree[j].unlocked)
                        return false;
                }
            }
        }
        return true;
    }

    public void ActivateSkill(int level, int skillID)
    {
        _bufferSkill = GetSkillData(skillID);

        if (level == 0 || _bufferSkill == null)
        {
            return;
        }
        if (level >= _bufferSkill.minimumLevel && CheckIfAvailable(_bufferSkill) && !_bufferSkill.unlocked)
        {
            _bufferSkill.unlocked = true;
        }
        else if (level < _bufferSkill.minimumLevel)
        {
            Debug.LogError("Minimum Level Not Achieved");
        }
    }

    public SkillData GetSkillData(int id)
    {
        _bufferSkill = null;
        foreach (SkillData data in skillTree)
        {
            if (data.skillID == id)
            {
                _bufferSkill = data;
                break;
            }
        }
        return _bufferSkill;
    }

    public List<SkillData> GetAvailableSkillsList()
    {
        availableSkillList.Clear();
        foreach (SkillData data in skillTree)
        {
            if (!data.unlocked && CheckIfAvailable(data))
                availableSkillList.Add(data);
        }
        return availableSkillList;
    }

    public void ClearUnlockData()
    {
        foreach (SkillData data in skillTree)
        {
            data.unlocked = false;
        }
    }
}

#endregion Declarations and Definitions

[CreateAssetMenu(fileName = "New Character Stats Data", menuName = "ScriptableObjects/CharacterStatsData")]
public class CharacterStats : ScriptableObject
{
    [SerializeField]
    public Stats stats;

    [SerializeField]
    public SkillTree skillTree;
}