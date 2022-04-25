using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public weaponData weaponData;
    public MeshRenderer mRenderer;
    public ParticleSystem itemParticle;
    public bool isEquippable;
    public bool isStarterPickup;
    public bool isTutorialWeapon;
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
        if (other.CompareTag("Player") && isEquippable && isStarterPickup && !isTutorialWeapon)
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
                if (itemParticle.isPlaying)
                {
                    itemParticle.Stop();
                }
            }
        }
        else if (other.CompareTag("Player") && isEquippable && !isStarterPickup && !isTutorialWeapon)
        {
            combatController combatController = other.GetComponent<combatController>();
            if (combatController.currentWeapon == null)
            {
                combatController.currentWeaponUsable = true;
                combatController.currentWeapon = weaponData;
                combatController.weaponEquipped = true;
                combatController.equipWeapon();
                Destroy(this.gameObject);
            }
        }
        else if (other.CompareTag("Player") && isEquippable && isTutorialWeapon)
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
                StartCoroutine("reenable");
                if (itemParticle.isPlaying)
                {
                    itemParticle.Stop();
                }
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
    public IEnumerator reenable()
    {
        yield return new WaitForSeconds(0.25f);
        enableMesh();
        isEquippable = true;
        itemParticle.Play();
        if (itemParticle.isStopped)
        {
            itemParticle.Play();
        }
    }
}
