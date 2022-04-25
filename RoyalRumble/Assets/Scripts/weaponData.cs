using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "weaponDataFile", menuName = "Weapon/Weapon Data")]
public class weaponData : ScriptableObject
{
    public enum weaponType { shield, spear, sword, gun }
    [Tooltip("May not be used, but can set regardless.")] public string weaponName;
    [Tooltip("Type of weapon being picked up.")] public weaponType thisWeaponType;
}
