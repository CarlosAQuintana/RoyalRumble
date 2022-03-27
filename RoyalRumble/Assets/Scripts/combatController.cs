using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class combatController : MonoBehaviour
{
    [SerializeField] public PlayerController player;
    [SerializeField] CharacterController controller;


    [Header("Weapons")] // Each weapon GameObject.
    public GameObject spear;
    public GameObject shield;
    public GameObject sword;
    public GameObject gun;
    public GameObject magicWand;

    [Header("Combat Variables")]
    [SerializeField] LayerMask playerLayer;
    public bool isDead;
    public weaponData currentWeapon; // Data for current weapon.
    public Transform hand; // Player hand location.
    public Transform attackPointOne; // Attack point.
    public bool currentWeaponUsable; // Has the weapons' use been exhausted?

    [Header("Spear Variables")]
    [SerializeField] private float thrustPower;
    [SerializeField] private float thrustDuration;
    public float hitboxRadius;
    public bool goSpearDash;
    float spearSmoothVelocityHolder;
    void Start()
    {
        hand = transform.Find("Hand");
        player = GetComponent<PlayerController>();
        attackPointOne = transform.Find("AttackPoint01");
    }
    void Update()
    {
        spearDash();
    }
    public void attack(InputAction.CallbackContext context)
    {
        if (!player.canMove) // Prevent atacking if movement is disabled.
        {
            return;
        }
        if (context.performed && currentWeaponUsable && currentWeapon != null) // Do a weapon attack when a weapon is equipped.
        {
            Debug.Log("Attack!");
            switch (currentWeapon.thisWeaponType) // Execute a specific attack based on weapoon equipped.
            {
                case weaponData.weaponType.spear:
                    StartCoroutine("spearAttack"); // Start the spear attack coroutine.
                    break;
                case weaponData.weaponType.sword:

                    break;
                case weaponData.weaponType.gun:

                    break;
                case weaponData.weaponType.shield:

                    break;
                case weaponData.weaponType.magicWand:

                    break;
            }
        }
        else if (context.performed && currentWeaponUsable && currentWeapon == null) // Do a punch attack when a weapon is not equipped.
        {

        }
    }

    #region Spear Combat
    public IEnumerator spearAttack()
    {
        currentWeaponUsable = false;
        player.canMove = false;
        goSpearDash = true;
        yield return new WaitForSeconds(.35f);
        currentWeaponUsable = true; // UNTIL THROWING IS IMPLEMENTED, MAKE SPEAR ATTACK RE-USABLE!
        goSpearDash = false;
        player.canMove = true;
    }
    private void spearDash()
    {
        if (goSpearDash)
        {
            controller.Move(transform.forward * Time.fixedDeltaTime * thrustPower);
            Collider[] spearCol = Physics.OverlapSphere(attackPointOne.position, hitboxRadius, playerLayer);
            if (spearCol.Length != 0)
            {
                goSpearDash = false;
                StopCoroutine("spearAttack");
                player.canMove = true;
            }
            for (int i = 0; i < spearCol.Length; i++)
            {
                Debug.Log("Hit " + spearCol.Length + " players!");
                combatController enemyCombat = spearCol[i].GetComponent<combatController>(); // Fetch the enemy's combatController,
                PlayerController enemyControl = spearCol[i].GetComponent<PlayerController>(); // enemy's PlayerController,
                roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
                rManager.numOfPlayersAlive -= spearCol.Length;
                rManager.playerIsDead[enemyControl.playerID] = true; // Set any player hit as dead...
                enemyCombat.player.canMove = false; // and disable their movement.
                rManager.checkForRoundWin();
                //enemyCombat.isDead = true;
            }
        }
    }
    #endregion

    public void equipWeapon()
    {
        switch (currentWeapon.thisWeaponType)
        {
            case weaponData.weaponType.spear:
                spear.SetActive(true);
                break;
        }
    }
    public void unEquipWeapon()
    {
        spear.SetActive(false);
        //shield.SetActive(false);
        //sword.SetActive(false);
        //gun.SetActive(false);
        //magicWand.SetActive(false);
        currentWeapon = null;
        currentWeaponUsable = false;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPointOne.position, hitboxRadius);
    }
}