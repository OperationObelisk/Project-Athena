using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Expandable]
    public CharacterStats charStats;

    [SerializeField]
    private List<SkillButton> skillButtons = null;

    [SerializeField]
    private List<SkillData> availableSkills = null;

    [SerializeField]
    private bool clearData = false;

    private void Start()
    {
        if (clearData)
        {
            charStats.skillTree.ClearUnlockData();
        }
        DisplayAvailableSkills();
    }

    public void UnlockSkill(int skillID)
    {
        charStats.skillTree.ActivateSkill(charStats.stats.AlterExperienceValue(), skillID);
        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (skillButtons[i].skillID == skillID)
            {
                skillButtons[i].buttonImageComponent.color = Color.green;
                break;
            }
        }
        DisplayAvailableSkills();
    }

    private void DisplayAvailableSkills()
    {
        availableSkills = charStats.skillTree.GetAvailableSkillsList();
        for (int i = 0; i < availableSkills.Count; i++)
        {
            for (int j = 0; j < skillButtons.Count; j++)
            {
                if (availableSkills[i].skillID == skillButtons[j].skillID)
                {
                    skillButtons[j].buttonImageComponent.color = Color.yellow;
                    skillButtons[j].buttonComponent.interactable = true;
                    break;
                }
            }
        }
    }
}