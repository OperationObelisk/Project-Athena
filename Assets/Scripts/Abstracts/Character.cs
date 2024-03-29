﻿using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData charStats;

    [SerializeField]
    private List<SkillButton> skillButtons = null;

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
        charStats.UnlockSkill(skillID);
        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (skillButtons[i].skillID == skillID)
            {
                skillButtons[i].buttonImageComponent.color = Color.green;
                skillButtons[i].buttonComponent.interactable = false;
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
                    if (availableSkills[i].minimumLevel > charStats.stats.AlterExperienceValue())
                    {
                        skillButtons[j].buttonImageComponent.color = Color.cyan;
                        skillButtons[j].buttonComponent.interactable = false;
                    }
                    else if (availableSkills[i].minimumLevel <= charStats.stats.AlterExperienceValue())
                    {
                        skillButtons[j].buttonImageComponent.color = Color.yellow;
                        skillButtons[j].buttonComponent.interactable = true;
                    }
                    break;
                }
            }
        }
    }
}