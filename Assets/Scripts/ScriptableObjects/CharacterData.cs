using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Declarations and Definitions

[System.Serializable]
public class Stats
{
    #region Actual Stats

    private float health = 0.0f;
    private float damage = 0.0f;
    private float endurance = 0.0f;
    private int experience = 0;
    private int level = 1;

    #endregion Actual Stats

    #region Buffer Calculation Variables

    private int _ExpBase = 10;
    private int _ExpLeft = 10;
    private float _ExpMod = 1.25f;
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

    public int AlterExperienceValue(int value = 0, bool resetValue = false, bool modifyLevelValue = false, int levelValue = 0, int experienceValue = 0)
    {
        if (resetValue == true)
        {
            level = 1;
            experience = 0;
            _ExpBase = 10;
            _ExpLeft = 10;
            _ExpMod = 1.25f;
            _t = 0.0f;
        }
        else if (modifyLevelValue == true)
        {
            level = levelValue;
            experience = experienceValue;
        }
        else
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
        }
        return level;
    }

    public int GetExperienceValue()
    {
        return experience;
    }

    #endregion Public Methods
}

[System.Serializable]
public class Ability
{
    private int ID;
    private string Name;
    private bool Unlocked;

#if UNITY_EDITOR
    public int abilityID;
    public string abilityName;
    public bool abilityUnlock;

    public void EditorSetAbilityData()
    {
        ID = abilityID;
        Name = abilityName;
        Unlocked = abilityUnlock;
    }

#endif

    public void GetAbilityData(out int id, out string name, out bool unlock)
    {
        id = ID;
        name = Name;
        unlock = Unlocked;
    }

    public void SetAbilityData(int id, string name, bool unlock)
    {
        ID = id;
        Name = name;
        Unlocked = unlock;
    }
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

    [System.Serializable]
    public enum StatModificationCategory
    {
        None = 0,
        Health = 1,
        Damage = 2,
        Endurance = 3,
    }

    [System.Serializable]
    public enum AbilityToBeUnlocked
    {
        Counter1 = 0,
        Counter2 = 1,
        Fight1 = 2,
    }

    public int skillID; //Unique Identifier Value for skill
    public string skillName;

    [TextArea]
    public string skillDescription;

    public bool unlocked = false;
    public int minimumLevel = 1;
    public List<int> skillPrerequisites; //List of unique Skill IDs which need to be unlocked before unlocking current skill
    public SkillType skillType;
    public StatModificationCategory statModificationCategory;
    public AbilityToBeUnlocked abilityToBeUnlocked;
    public float additivePercentage;
}

[System.Serializable]
public class SkillTree
{
    #region Buffer Calculation Variables

    private SkillData _bufferSkill = null;
    private List<SkillData> availableSkillList = null;

    #endregion Buffer Calculation Variables

    public List<SkillData> skillTree;

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
    }

    public void ClearUnlockData()
    {
        foreach (SkillData data in skillTree)
        {
            data.unlocked = false;
        }
    }

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
}

#endregion Declarations and Definitions

[CreateAssetMenu(fileName = "Character Data Values", menuName = "ScriptableObjects/CharacterDataValues")]
public class CharacterData : ScriptableObject
{
    [SerializeField]
    public Stats stats;

    [SerializeField]
    public List<Ability> abilities;

    [SerializeField]
    public SkillTree skillTree;

    public void UnlockSkill(int skillID)
    {
        skillTree.ActivateSkill(stats.AlterExperienceValue(), skillID);
        SkillData data = skillTree.GetSkillData(skillID);
        float value;
        switch (data.skillType)
        {
            case SkillData.SkillType.Passive:
                switch (data.statModificationCategory)
                {
                    case SkillData.StatModificationCategory.Health:
                        value = stats.AlterHealthValue();
                        value = (value * (data.additivePercentage / 100));
                        stats.AlterHealthValue(value);
                        break;

                    case SkillData.StatModificationCategory.Damage:
                        value = stats.AlterDamageValue();
                        value = (value * (data.additivePercentage / 100));
                        stats.AlterDamageValue(value);
                        break;

                    case SkillData.StatModificationCategory.Endurance:
                        value = stats.AlterEnduranceValue();
                        value = (value * (data.additivePercentage / 100));
                        stats.AlterEnduranceValue(value);
                        break;
                }
                break;
        }
    }
}