using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPickupManager : MonoBehaviour
{
    [Header("Level One")]
    [SerializeField] private Transform[] levelOneSpawns;
    [SerializeField] private int numLevelOneSpawns;

    void Start()
    {

    }
    void Update()
    {

    }
    public void findWeaponSpawns()
    {

        Transform weaponSpawnLevelOne = GameObject.Find("LVL One Weapon Spawns (Parent)").GetComponent<Transform>(); // The parent gameobject housing weapon locations.
        levelOneSpawns = new Transform[6];
        for (int t = 0; t < levelOneSpawns.Length; t++)
        {
            levelOneSpawns[t] = weaponSpawnLevelOne.Find("ws" + t);
        }
    }
}
