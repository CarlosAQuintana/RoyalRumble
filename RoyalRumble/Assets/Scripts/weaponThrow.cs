using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class weaponThrow : MonoBehaviour
{
    [Header("Script and Component References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private combatController combat;

    [Header("Prefabs")]
    [SerializeField] private GameObject spearProjectilePrefab;

    [Header("Throwing Variables")]
    private bool throwStarted;
    [SerializeField] private float throwPower;
    [SerializeField] private float throwScale;
    void Start()
    {
        player = GetComponent<PlayerController>();
        combat = GetComponent<combatController>();
        throwStarted = false;
    }
    void Update()
    {

    }
    public void tossWeapon(InputAction.CallbackContext context) // Select a throwing action based on type of weapon.
    {
        if (context.started && combat.currentWeapon != null)
        {
            switch (combat.currentWeapon.thisWeaponType)
            {
                case weaponData.weaponType.spear:
                    spearThrow();
                    break;
            }
        }
        if (context.canceled && throwStarted)
        {
            throwStarted = false;
        }

    }
    public void spearThrow() // Spawn the projectile and set it's speed.
    {
        Vector3 modSpawn = new Vector3(combat.attackPointOne.position.x, combat.attackPointOne.position.y, combat.attackPointOne.position.z + 2f);
        GameObject temp = spearProjectilePrefab;
        Instantiate(temp, modSpawn, transform.rotation);
        projectile proj = temp.GetComponent<projectile>();
        proj.owner = player;
        proj.speed = throwPower * throwScale;
        combat.unEquipWeapon();
    }

}
