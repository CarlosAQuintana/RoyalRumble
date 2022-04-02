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
        mRenderer = GetComponent<MeshRenderer>();
    }
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isEquippable)
        {
            disableMesh();
            isEquippable = false;
            combatController combatController = other.GetComponent<combatController>();
            combatController.currentWeaponUsable = true;
            combatController.currentWeapon = weaponData;
            combatController.equipWeapon();
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
