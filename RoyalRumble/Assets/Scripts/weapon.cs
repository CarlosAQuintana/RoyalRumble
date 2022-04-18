using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public weaponData weaponData;
    public MeshRenderer mRenderer;
    public bool isEquippable;
    void Start()
    {
        isEquippable = true;
        //mRenderer = GetComponent<MeshRenderer>();
    }
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isEquippable)
        {
            combatController combatController = other.GetComponent<combatController>();
            if (combatController.currentWeapon == null)
            {
                disableMesh();
                isEquippable = false;
                combatController.currentWeaponUsable = true;
                combatController.currentWeapon = weaponData;
                combatController.weaponEquipped = true;
                combatController.equipWeapon();
            }
        }
    }
    public void enableMesh()
    {
        mRenderer.enabled = true;
    }
    public void disableMesh()
    {
        mRenderer.enabled = false;
    }
}
