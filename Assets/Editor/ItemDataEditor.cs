using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using EditorExtensions;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = target as ItemData;
        script.itemID = EditorGUILayout.IntField("Item ID", script.itemID);
        EditorFunctionCollections.DrawLine();
        script.itemName = EditorGUILayout.TextField("Item Name", script.itemName);
        EditorGUILayout.LabelField("Item Description:-");
        EditorStyles.textArea.wordWrap = true;
        script.itemDescription = EditorGUILayout.TextArea(script.itemDescription, EditorStyles.textArea, GUILayout.MaxHeight(45));
        script.mesh = (GameObject)EditorGUILayout.ObjectField("Item Mesh", script.mesh, typeof(GameObject), true);
        EditorFunctionCollections.DrawLine();
        script.itemType = (ItemType)EditorGUILayout.EnumPopup("ItemType", script.itemType);
        switch (script.itemType)
        {
            case ItemType.Weapon:
                script.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", script.weaponType);
                break;

            case ItemType.Armor:
                script.armorSlot = (ArmorSlot)EditorGUILayout.EnumPopup("Armor Slot", script.armorSlot);
                break;

            case ItemType.Potion:
                script.potionType = (PotionType)EditorGUILayout.EnumPopup("Potion Type", script.potionType);
                break;
        }
    }
}