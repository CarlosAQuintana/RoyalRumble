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
    [SerializeField] private float throwPower;
    [SerializeField] private float throwScale;
    void Start()
    {
        player = GetComponent<PlayerController>();
        combat = GetComponent<combatController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void tossWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (combat.currentWeapon != null)
                switch (combat.currentWeapon.thisWeaponType)
                {
                    case weaponData.weaponType.spear:
                        spearThrow();
                        break;
                }
        }
    }
    public void spearThrow()
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
