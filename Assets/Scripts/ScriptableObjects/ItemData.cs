using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Declarations and Definitions

[System.Serializable]
public enum ItemType
{
    Weapon = 0,
    Armor = 1,
    Potion = 2,
}

public enum WeaponType
{
    Sword = 0,
    LongSword = 1,
    Dagger = 2,
    Mace = 3,
    Warhammer = 4,
}

public enum ArmorSlot
{
    Head = 0,
    Torso = 1,
    Hand = 2,
    Legs = 3,
    Shield = 4,
}

public enum PotionType
{
    Health = 0,
    Invincibility = 1,
}

#endregion Declarations and Definitions

[CreateAssetMenu(fileName = "New ItemData", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    public int itemID = 0;
    public string itemName = null;

    [Multiline()]
    public string itemDescription = null;

    public GameObject mesh = null;
    public ItemType itemType;
    public WeaponType weaponType;
    public ArmorSlot armorSlot;
    public PotionType potionType;
}