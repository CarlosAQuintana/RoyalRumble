using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public weaponData weaponData;
    public bool isEquippable;
    void Start()
    {
        isEquippable = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isEquippable)
        {
            isEquippable = false;
            combatController combatController = other.GetComponent<combatController>();
            combatController.hasWeapon = true;
            combatController.currentWeaponUsable = true;
            combatController.currentWeapon = weaponData;
            combatController.equipWeapon();
        }
    }
}
