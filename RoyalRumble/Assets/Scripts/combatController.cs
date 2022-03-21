using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class combatController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] CharacterController controller;
    public weaponData currentWeapon;

    [Header("Weapons")]
    public GameObject spear;
    public GameObject shield;
    public GameObject sword;
    public GameObject gun;
    public GameObject magicWand;

    [Header("Combat Variables")]
    public Transform hand;
    public bool currentWeaponUsable;
    public bool hasWeapon;

    [Header("Spear Variables")]
    public float spearSmoothTime = .65f;
    public bool goSpearDash;
    float spearSmoothVelocityHolder;
    void Start()
    {
        hand = transform.Find("Hand");
        player = GetComponent<PlayerController>();
        //controller.GetComponent<CharacterController>();
        //spear.SetActive(false);
    }
    void Update()
    {
        spearDash();
    }
    public void attack(InputAction.CallbackContext context)
    {
        if (!currentWeapon || !player.canMove)
        {
            return;
        }
        if (context.performed && currentWeaponUsable)
        {
            Debug.Log("Attack!");
            switch (currentWeapon.thisWeaponType)
            {
                case weaponData.weaponType.spear:
                    StartCoroutine("spearAttack");
                    break;
            }
        }
    }
    public IEnumerator spearAttack()
    {
        player.canMove = false;
        //float moveTime = Mathf.SmoothDamp(0f, 50f, ref spearSmoothVelocityHolder, spearSmoothTime);
        goSpearDash = true;
        yield return new WaitForSeconds(.35f);
        goSpearDash = false;
        currentWeaponUsable = false;
        player.canMove = true;
    }
    private void spearDash()
    {
        if (goSpearDash)
            controller.Move(transform.forward * Time.fixedDeltaTime * 5f);
    }
    public void equipWeapon()
    {
        switch (currentWeapon.thisWeaponType)
        {
            case weaponData.weaponType.spear:
                spear.SetActive(true);
                break;
        }
    }
}
