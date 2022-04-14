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
    public Transform leftHand;
    public Transform attackPointOne; // Attack point.
    public Transform attackPointTwo;
    public bool weaponEquipped;
    public bool currentWeaponUsable; // Has the weapons' use been exhausted?

    [Header("Spear Variables")]
    [SerializeField] private float thrustPower;
    [SerializeField] private float thrustDuration;
    public float spearHitRadius;
    public bool goSpearDash;
    float spearSmoothVelocityHolder;

    [Header("Shield Variables")]
    [SerializeField] private float blitzPower;
    [SerializeField] private float blitzDuration;
    public bool goShieldBlitz;

    [Header("Sword Variables")]
    [SerializeField] private GameObject bladeBeam;
    public float slashDuration;
    public float swordStepPower;
    public float slashRange;
    public bool goSwordSlash;

    void Awake()
    {

    }
    void Start()
    {
        hand = transform.Find("Hand");
        leftHand = transform.Find("Left Hand");
        player = GetComponent<PlayerController>();
        attackPointOne = transform.Find("AttackPoint01");
        attackPointTwo = transform.Find("AttackPoint02");
    }
    void Update()
    {
        spearDash();
        shieldBlitz();
        swordAction();
    }
    public void killPlayer(combatController targetCombat, PlayerController targetController)
    {
        roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
        rManager.numOfPlayersAlive--;
        rManager.playerIsDead[targetController.playerID] = true; // Set any player hit as dead...
        targetCombat.isDead = true;
        targetCombat.player.canMove = false; // and disable their movement.
        rManager.checkForRoundWin();
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
            switch (currentWeapon.thisWeaponType) // Execute a specific attack based on weapon equipped.
            {
                case weaponData.weaponType.spear:
                    StartCoroutine("spearAttack");
                    break;
                case weaponData.weaponType.shield:
                    StartCoroutine("shieldAttack");
                    break;
                case weaponData.weaponType.gun:

                    break;
                case weaponData.weaponType.sword:
                    StartCoroutine("swordAttack");
                    break;
            }
        }
        else if (context.performed && currentWeapon == null) // Do a punch attack when a weapon is not equipped.
        {
            StartCoroutine("punch");
        }
    }
    #region unarmed Combat
    public IEnumerator punch()
    {
        player.canMove = false;
        RaycastHit ray;
        Debug.DrawRay(attackPointTwo.position, transform.forward, Color.yellow, spearHitRadius);
        if (Physics.Raycast(attackPointTwo.position, transform.forward, out ray, .5f, playerLayer))
        {
            combatController enemyCombat = ray.collider.GetComponent<combatController>(); // Fetch the enemy's combatController,
            PlayerController enemyControl = ray.collider.GetComponent<PlayerController>(); // enemy's PlayerController,
            roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
            killPlayer(enemyCombat, enemyControl);
            StopCoroutine("punch");
            //enemyCombat.isDead = true;
        }
        yield return new WaitForSeconds(.5f);
        player.canMove = true;
    }
    #endregion
    #region Spear Combat
    private void spearDash()
    {
        if (goSpearDash)
        {
            controller.Move(transform.forward * Time.fixedDeltaTime * thrustPower);
            RaycastHit ray;
            if (Physics.Raycast(attackPointOne.position, transform.forward, out ray, spearHitRadius, playerLayer))
            {
                goSpearDash = false;
                StopCoroutine("spearAttack");
                player.canMove = true;
                combatController enemyCombat = ray.collider.GetComponent<combatController>(); // Fetch the enemy's combatController,
                PlayerController enemyControl = ray.collider.GetComponent<PlayerController>(); // enemy's PlayerController,
                roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
                killPlayer(enemyCombat, enemyControl);
                rManager.checkForRoundWin();
                //enemyCombat.isDead = true;
            }
        }
    }
    public IEnumerator spearAttack()
    {
        currentWeaponUsable = false;
        player.canMove = false;
        goSpearDash = true;
        yield return new WaitForSeconds(thrustDuration);
        goSpearDash = false;
        player.canMove = true;
    }
    #endregion
    #region Shield Combat
    private void shieldBlitz()
    {
        if (goShieldBlitz)
        {
            controller.Move(transform.forward * Time.fixedDeltaTime * blitzPower);
            RaycastHit ray;
            if (Physics.Raycast(attackPointOne.position, transform.forward, out ray, spearHitRadius, playerLayer))
            {
                goShieldBlitz = false;
                StopCoroutine("shieldAttack");
                player.canMove = true;
                combatController enemyCombat = ray.collider.GetComponent<combatController>(); // Fetch the enemy's combatController,
                PlayerController enemyControl = ray.collider.GetComponent<PlayerController>(); // enemy's PlayerController,
                roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
                killPlayer(enemyCombat, enemyControl);
                rManager.checkForRoundWin();
                //enemyCombat.isDead = true;
            }
        }
    }
    public IEnumerator shieldAttack()
    {
        currentWeaponUsable = false;
        player.canMove = false;
        goShieldBlitz = true;
        yield return new WaitForSeconds(blitzDuration);
        goShieldBlitz = false;
        player.canMove = true;
    }
    #endregion
    #region Sword Attack
    public void swordAction()
    {
        if (goSwordSlash)
        {
            controller.Move(transform.forward * Time.fixedDeltaTime * swordStepPower);
            RaycastHit ray;
            if (Physics.Raycast(attackPointOne.position, transform.forward, out ray, slashDuration, playerLayer))
            {
                goShieldBlitz = false;
                StopCoroutine("swordAttack");
                player.canMove = true;
                combatController enemyCombat = ray.collider.GetComponent<combatController>(); // Fetch the enemy's combatController,
                PlayerController enemyControl = ray.collider.GetComponent<PlayerController>(); // enemy's PlayerController,
                roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
                killPlayer(enemyCombat, enemyControl);
                rManager.checkForRoundWin();
                //enemyCombat.isDead = true;
            }
        }
    }
    public IEnumerator swordAttack()
    {
        currentWeaponUsable = false;
        player.canMove = false;
        goSwordSlash = true;
        yield return new WaitForSeconds(slashDuration);
        goSwordSlash = false;
        player.canMove = true;
    }
    #endregion
    public void equipWeapon()
    {
        switch (currentWeapon.thisWeaponType)
        {
            case weaponData.weaponType.spear:
                spear.SetActive(true);
                break;
            case weaponData.weaponType.shield:
                shield.SetActive(true);
                break;
            case weaponData.weaponType.sword:
                sword.SetActive(true);
                break;
            case weaponData.weaponType.gun:
                gun.SetActive(true);
                break;
        }
    }
    public void unEquipWeapon()
    {
        spear.SetActive(false);
        shield.SetActive(false);
        sword.SetActive(false);
        //gun.SetActive(false);
        //magicWand.SetActive(false);
        currentWeapon = null;
        weaponEquipped = false;
        currentWeaponUsable = false;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPointOne.position, spearHitRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPointTwo.position, spearHitRadius / 2f);
    }
}