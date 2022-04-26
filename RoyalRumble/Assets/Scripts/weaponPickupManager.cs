using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPickupManager : MonoBehaviour
{
    public roundManager round;
    public float timeSinceLastSpawn;
    public float spawnInterval;
    public float spawnPointRange;
    public GameObject[] weapons;

    [Header("Level One")]
    [SerializeField] private Transform[] levelOneSpawns;

    [Header("Level Two")]
    [SerializeField] private Transform[] levelTwoSpawns;

    [Header("Level Three")]
    [SerializeField] private Transform[] levelThreeSpawns;

    [Header("Level Four")]
    [SerializeField] private Transform[] levelFourSpawns;

    void Start()
    {

    }
    void Update()
    {
        if (round.RoundIsInPlay)
        {
            if (timeSinceLastSpawn > spawnInterval)
            {
                timeSinceLastSpawn = 0;
                spawnWeapon();
            }
            timeSinceLastSpawn += Time.deltaTime;
        }
    }
    private void spawnWeapon()
    {
        int weaponToSpawn = Random.Range(0, weapons.Length - 1);
        int spawnPointToUse = 0;
        Debug.Log("" + weaponToSpawn);
        switch (round.currentLevel)
        {
            case roundManager.level.castle:
                spawnPointToUse = Random.Range(0, levelOneSpawns.Length - 1);
                Transform sPointOne = levelOneSpawns[spawnPointToUse];
                GameObject pickup1 = Instantiate(weapons[weaponToSpawn], sPointOne.position, Quaternion.identity);
                break;
            case roundManager.level.ice:
                spawnPointToUse = Random.Range(0, levelTwoSpawns.Length - 1);
                Transform sPointTwo = levelTwoSpawns[spawnPointToUse];
                GameObject pickup2 = Instantiate(weapons[weaponToSpawn], sPointTwo.position, Quaternion.identity);
                break;
            case roundManager.level.jungle:
                spawnPointToUse = Random.Range(0, levelThreeSpawns.Length - 1);
                Transform sPointThree = levelThreeSpawns[spawnPointToUse];
                GameObject pickup3 = Instantiate(weapons[weaponToSpawn], sPointThree.position, Quaternion.identity);
                break;
            case roundManager.level.fire:
                spawnPointToUse = Random.Range(0, levelFourSpawns.Length - 1);
                Transform sPointFour = levelFourSpawns[spawnPointToUse];
                GameObject pickup4 = Instantiate(weapons[weaponToSpawn], sPointFour.position, Quaternion.identity);
                break;
        }
    }
    private void OnDrawGizmosSelected()
    {
        foreach (Transform spawnPoint in levelOneSpawns)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(spawnPoint.position, spawnPointRange);
        }
    }
}
