using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using EditorExtensions;

[CustomEditor(typeof(CharacterData))]
public class CharacterDataEditor : Editor
{
    private CharacterData script;
    private GUIStyle richTextStyle;

    private int healthAlterValue = 0;
    private int damageAlterValue = 0;
    private int enduranceAlterValue = 0;
    private int experienceAlterValue = 0;

    private int selectedSkillTreeIndex = 10;

    public override void OnInspectorGUI()
    {
        script = target as CharacterData;
        richTextStyle = new GUIStyle(GUI.skin.label)
        {
            richText = true
        };
        EditorStyles.textField.alignment = TextAnchor.MiddleLeft;
        EditorStyles.label.alignment = TextAnchor.MiddleLeft;
        EditorFunctionCollections.DrawLine();
        StatsVisualHandler();
        EditorFunctionCollections.DrawLine();
        AbilitiesVisualHandler();
        EditorFunctionCollections.DrawLine();
        SkillTreeVisualHandler();
        EditorFunctionCollections.DrawLine();
    }

    private void StatsVisualHandler()
    {
        EditorGUILayout.LabelField("<b><size=15>STATS</size></b>", richTextStyle);
        StatsAlterButtons("HEALTH");
        StatsAlterButtons("DAMAGE");
        StatsAlterButtons("ENDURANCE");
        StatsAlterButtons("EXPERIENCE");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LEVEL", GUILayout.MaxWidth(75), GUILayout.MaxHeight(20));
        EditorGUILayout.LabelField(script.stats.AlterExperienceValue().ToString(), GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Clear Data"))
        {
            script.stats.AlterHealthValue(-script.stats.AlterHealthValue());
            script.stats.AlterDamageValue(-script.stats.AlterDamageValue());
            script.stats.AlterEnduranceValue(-script.stats.AlterEnduranceValue());
            script.stats.AlterExperienceValue(0, true);
        }
    }

    private void StatsAlterButtons(string stat)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(stat, GUILayout.MaxWidth(75), GUILayout.MaxHeight(20));
        switch (stat)
        {
            case "HEALTH":
                EditorGUILayout.LabelField(script.stats.AlterHealthValue().ToString(), GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
                break;

            case "DAMAGE":
                EditorGUILayout.LabelField(script.stats.AlterDamageValue().ToString(), GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
                break;

            case "ENDURANCE":
                EditorGUILayout.LabelField(script.stats.AlterEnduranceValue().ToString(), GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
                break;

            case "EXPERIENCE":
                EditorGUILayout.LabelField(script.stats.GetExperienceValue().ToString(), GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
                break;
        }
        if (GUILayout.Button("+", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
        {
            switch (stat)
            {
                case "HEALTH":
                    script.stats.AlterHealthValue(healthAlterValue);
                    break;

                case "DAMAGE":
                    script.stats.AlterDamageValue(damageAlterValue);
                    break;

                case "ENDURANCE":
                    script.stats.AlterEnduranceValue(enduranceAlterValue);
                    break;

                case "EXPERIENCE":
                    script.stats.AlterExperienceValue(experienceAlterValue);
                    break;
            }
        }
        if (GUILayout.Button("-", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
        {
            switch (stat)
            {
                case "HEALTH":
                    script.stats.AlterHealthValue(-healthAlterValue);
                    break;

                case "DAMAGE":
                    script.stats.AlterDamageValue(-damageAlterValue);
                    break;

                case "ENDURANCE":
                    script.stats.AlterEnduranceValue(-enduranceAlterValue);
                    break;

                case "EXPERIENCE":
                    script.stats.AlterExperienceValue(-experienceAlterValue);
                    break;
            }
        }
        switch (stat)
        {
            case "HEALTH":
                healthAlterValue = EditorGUILayout.IntField(healthAlterValue, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
                break;

            case "DAMAGE":
                damageAlterValue = EditorGUILayout.IntField(damageAlterValue, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
                break;

            case "ENDURANCE":
                enduranceAlterValue = EditorGUILayout.IntField(enduranceAlterValue, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
                break;

            case "EXPERIENCE":
                experienceAlterValue = EditorGUILayout.IntField(experienceAlterValue, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
                break;
        }
        EditorGUILayout.EndHorizontal();
    }

    private void AbilitiesVisualHandler()
    {
        EditorGUILayout.LabelField("<b><size=15>ABILITIES</size></b>", richTextStyle);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ID", GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
        EditorGUILayout.LabelField("NAME", GUILayout.MaxWidth(80), GUILayout.MaxHeight(20));
        EditorGUILayout.LabelField("IS UNLOCKED", GUILayout.MaxWidth(80), GUILayout.MaxHeight(20));
        EditorGUILayout.EndHorizontal();
        for (int i = 0; i < script.abilities.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            script.abilities[i].abilityID = EditorGUILayout.IntField(script.abilities[i].abilityID, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
            script.abilities[i].abilityName = EditorGUILayout.TextField(script.abilities[i].abilityName, GUILayout.MaxWidth(80), GUILayout.MaxHeight(20));
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(40), GUILayout.MaxHeight(20));
            script.abilities[i].abilityUnlock = EditorGUILayout.Toggle(script.abilities[i].abilityUnlock, GUILayout.MaxWidth(40), GUILayout.MaxHeight(20));
            if (GUILayout.Button("✔", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                script.abilities[i].EditorSetAbilityData();
            if (GUILayout.Button("-", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                script.abilities.Remove(script.abilities[i]);

            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add New Ability"))
            script.abilities.Add(new Ability());
    }

    private void SkillTreeVisualHandler()
    {
        EditorGUILayout.LabelField("<b><size=15>SKILL TREE</size></b>", richTextStyle);
        EditorGUILayout.Space();
        SkillDataParser();
    }

    private void SkillDataParser()
    {
        EditorGUILayout.LabelField("<b><size=10>Skill Data Parser</size></b>", richTextStyle);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Skill ID:", GUILayout.MaxWidth(45), GUILayout.MaxHeight(20));
        script.skillTree.skillTree[selectedSkillTreeIndex].skillID = EditorGUILayout.IntField(script.skillTree.skillTree[selectedSkillTreeIndex].skillID, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
        EditorGUILayout.LabelField("", GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
        EditorGUILayout.LabelField("Skill Name:", GUILayout.MaxWidth(65), GUILayout.MaxHeight(20));
        script.skillTree.skillTree[selectedSkillTreeIndex].skillName = EditorGUILayout.TextField(script.skillTree.skillTree[selectedSkillTreeIndex].skillName, GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("Skill Description:-");
        EditorStyles.textArea.wordWrap = true;
        script.skillTree.skillTree[selectedSkillTreeIndex].skillDescription = EditorGUILayout.TextArea(script.skillTree.skillTree[selectedSkillTreeIndex].skillDescription, EditorStyles.textArea, GUILayout.MaxHeight(45));
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Minimum Level Required:", GUILayout.MaxWidth(140), GUILayout.MaxHeight(20));
        script.skillTree.skillTree[selectedSkillTreeIndex].minimumLevel = EditorGUILayout.IntField(script.skillTree.skillTree[selectedSkillTreeIndex].minimumLevel, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
        EditorGUILayout.EndHorizontal();
        EditorFunctionCollections.DrawLine(true);
        EditorGUILayout.LabelField("Pre-requisite Skill IDs for unlocking:-");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ID", GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
        EditorGUILayout.LabelField("", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20));
        EditorGUILayout.LabelField("SKILL NAME", GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
        EditorGUILayout.EndHorizontal();
        for (int i = 0; i < script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites[i] = EditorGUILayout.IntField(script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites[i], GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
            string skillName = script.skillTree.GetSkillData(script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites[i]).skillName;
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20));
            EditorGUILayout.LabelField(skillName, GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
            if (GUILayout.Button("-", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
            {
                script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites.Remove(script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites[i]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}