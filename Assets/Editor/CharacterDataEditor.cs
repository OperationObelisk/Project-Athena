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

    private int selectedSkillTreeIndex = 0;

    public override void OnInspectorGUI()
    {
        script = target as CharacterData;
        richTextStyle = new GUIStyle(GUI.skin.label)
        {
            richText = true
        };
        EditorStyles.textField.alignment = TextAnchor.MiddleLeft;
        EditorStyles.label.alignment = TextAnchor.MiddleLeft;
        if (GUILayout.Button("SAVE DATA"))
            SaveData();
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
                healthAlterValue = EditorGUILayout.IntField(healthAlterValue, GUILayout.MaxWidth(40), GUILayout.MaxHeight(20));
                break;

            case "DAMAGE":
                damageAlterValue = EditorGUILayout.IntField(damageAlterValue, GUILayout.MaxWidth(40), GUILayout.MaxHeight(20));
                break;

            case "ENDURANCE":
                enduranceAlterValue = EditorGUILayout.IntField(enduranceAlterValue, GUILayout.MaxWidth(40), GUILayout.MaxHeight(20));
                break;

            case "EXPERIENCE":
                experienceAlterValue = EditorGUILayout.IntField(experienceAlterValue, GUILayout.MaxWidth(40), GUILayout.MaxHeight(20));
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
        EditorGUILayout.LabelField("UNLOCKED", GUILayout.MaxWidth(80), GUILayout.MaxHeight(20));
        EditorGUILayout.LabelField("OPERATORS", GUILayout.MaxWidth(80), GUILayout.MaxHeight(20));
        EditorGUILayout.EndHorizontal();
        if (script.abilities != null)
        {
            for (int i = 0; i < script.abilities.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                script.abilities[i].abilityID = EditorGUILayout.IntField(script.abilities[i].abilityID, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
                script.abilities[i].abilityName = EditorGUILayout.TextField(script.abilities[i].abilityName, GUILayout.MaxWidth(80), GUILayout.MaxHeight(20));
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20));
                EditorGUILayout.LabelField(script.abilities[i].abilityUnlock.ToString(), GUILayout.MaxWidth(60), GUILayout.MaxHeight(20));
                script.abilities[i].abilityUnlock = EditorGUILayout.Toggle(script.abilities[i].abilityUnlock, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20));
                if (GUILayout.Button("✔", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                    script.abilities[i].EditorSetAbilityData();

                if (GUILayout.Button("-", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                    script.abilities.Remove(script.abilities[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
        if (GUILayout.Button("Add New Ability"))
        {
            script.abilities.Add(new Ability());
            Repaint();
        }
    }

    private void SkillTreeVisualHandler()
    {
        EditorGUILayout.LabelField("<b><size=15>SKILL TREE</size></b>", richTextStyle);
        EditorGUILayout.Space();
        SkillDataParser();
        EditorFunctionCollections.DrawLine(true, 2f);
        SkillTreeParser();
    }

    private void SkillDataParser()
    {
        if (script.skillTree.skillTree != null && script.skillTree.skillTree.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<b><size=10>Skill Data Parser</size></b>", richTextStyle, GUILayout.MaxWidth(100));
            selectedSkillTreeIndex = EditorGUILayout.IntSlider(selectedSkillTreeIndex, 0, script.skillTree.skillTree.Count - 1);
            EditorGUILayout.EndHorizontal();
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
            EditorGUILayout.LabelField("Is Skill Unlocked:", GUILayout.MaxWidth(95), GUILayout.MaxHeight(20));
            script.skillTree.skillTree[selectedSkillTreeIndex].unlocked = EditorGUILayout.Toggle(script.skillTree.skillTree[selectedSkillTreeIndex].unlocked, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20));
            EditorGUILayout.EndHorizontal();
            EditorFunctionCollections.DrawLine(true);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Minimum Level Required:", GUILayout.MaxWidth(140), GUILayout.MaxHeight(20));
            script.skillTree.skillTree[selectedSkillTreeIndex].minimumLevel = EditorGUILayout.IntField(script.skillTree.skillTree[selectedSkillTreeIndex].minimumLevel, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("Pre-requisite Skill IDs for unlocking:-");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ID", GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20));
            EditorGUILayout.LabelField("SKILL NAME", GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
            EditorGUILayout.EndHorizontal();

            if (script.skillTree.skillTree != null && script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites != null)
            {
                for (int i = 0; i < script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites[i] = EditorGUILayout.IntField(script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites[i], GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
                    string skillName = script.skillTree.GetSkillData(script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites[i]).skillName;
                    EditorGUILayout.LabelField("", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20));
                    EditorGUILayout.LabelField(skillName, GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
                    if (GUILayout.Button("-", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                        script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites.Remove(script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites[i]);
                    EditorGUILayout.EndHorizontal();
                }
            }
            if (GUILayout.Button("Add New Pre-requisite"))
                script.skillTree.skillTree[selectedSkillTreeIndex].skillPrerequisites.Add(new int());
            EditorFunctionCollections.DrawLine(true);
            if (script.skillTree.skillTree != null)
                script.skillTree.skillTree[selectedSkillTreeIndex].skillType = (SkillData.SkillType)EditorGUILayout.EnumPopup("Skill Type:", script.skillTree.skillTree[selectedSkillTreeIndex].skillType);
            switch (script.skillTree.skillTree[selectedSkillTreeIndex].skillType)
            {
                case SkillData.SkillType.Passive:
                    script.skillTree.skillTree[selectedSkillTreeIndex].statModificationCategory = (SkillData.StatModificationCategory)EditorGUILayout.EnumPopup("Stat to be modified", script.skillTree.skillTree[selectedSkillTreeIndex].statModificationCategory);
                    script.skillTree.skillTree[selectedSkillTreeIndex].additivePercentage = EditorGUILayout.FloatField("Percentage to be added", script.skillTree.skillTree[selectedSkillTreeIndex].additivePercentage);
                    break;

                case SkillData.SkillType.Ability:
                    script.skillTree.skillTree[selectedSkillTreeIndex].abilityID = EditorGUILayout.IntField("Ability ID", script.skillTree.skillTree[selectedSkillTreeIndex].abilityID);
                    break;
            }
        }
    }

    private void SkillTreeParser()
    {
        EditorGUILayout.LabelField("<b><size=10>Skill Tree List Parser</size></b>", richTextStyle);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("OPERATORS", GUILayout.MaxWidth(75), GUILayout.MaxHeight(20));
        EditorGUILayout.LabelField("ID", GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
        EditorGUILayout.LabelField("NAME", GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
        EditorGUILayout.LabelField("PRE-REQUISITE IDS LIST");
        EditorGUILayout.EndHorizontal();
        if (script.skillTree.skillTree != null && script.skillTree.skillTree.Count > 0)
        {
            DisplaySkillList();
        }
        if (GUILayout.Button("Add New Skill"))
        {
            SkillData skill = new SkillData();
            skill.skillPrerequisites = new List<int>();
            script.skillTree.skillTree.Add(skill);
            Repaint();
        }
    }

    private void DisplaySkillList()
    {
        SkillTree _buffer = script.skillTree;
        for (int i = 0; i < script.skillTree.skillTree.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(10), GUILayout.MaxHeight(20));
            if (GUILayout.Button("-", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                script.skillTree.skillTree.Remove(script.skillTree.skillTree[i]);
            if (GUILayout.Button("◉", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                selectedSkillTreeIndex = i;
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(15), GUILayout.MaxHeight(20));
            if (i < script.skillTree.skillTree.Count)
            {
                EditorGUILayout.LabelField(_buffer.skillTree[i].skillID.ToString(), GUILayout.MaxWidth(30), GUILayout.MaxHeight(20));
                EditorGUILayout.LabelField(_buffer.skillTree[i].skillName, GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
                if (_buffer.skillTree[i].skillPrerequisites != null)
                    for (int j = 0; j < _buffer.skillTree[i].skillPrerequisites.Count; j++)
                        if (GUILayout.Button(_buffer.skillTree[i].skillPrerequisites[j].ToString(), GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                        {
                            SkillData data = _buffer.GetSkillData(_buffer.skillTree[i].skillPrerequisites[j]);
                            selectedSkillTreeIndex = _buffer.skillTree.IndexOf(data);
                        }
                //EditorGUILayout.LabelField(_buffer.skillTree[i].skillPrerequisites[j].ToString(), GUILayout.MaxWidth(10), GUILayout.MaxHeight(20));
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void SaveData()
    {
        AssetDatabase.Refresh();
        EditorUtility.SetDirty(script);
        AssetDatabase.SaveAssets();
    }
}