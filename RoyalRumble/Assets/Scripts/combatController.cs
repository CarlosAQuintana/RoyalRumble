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

    [Header("Combat Variables")]
    [SerializeField] LayerMask playerLayer;
    public bool isDead;
    public bool isKillable;
    public bool inTutorial;
    public weaponData currentWeapon; // Data for current weapon.
    public Transform hand; // Player hand location.
    public Transform leftHand;

    public bool weaponEquipped;
    public bool currentWeaponUsable; // Has the weapons' use been exhausted?

    [Header("Attack Points")]
    public Transform attackPointOne; // Attack point.
    public Transform attackPointTwo;
    public Transform attackPointThree;
    public Transform attackPointFour;

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
    public float slashDuration;
    public float swordStepPower;
    public float slashRange;
    public bool goSwordSlash;

    [Header("Gun Variables")]
    public GameObject bulletPrefab;
    public bool canShoot;
    public float reloadTime;
    public float reloadTimeElapsed;
    public float shotDuration;
    public float shotPower;
    public int maxShots;
    public int shotsLeft;

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
        attackPointThree = transform.Find("AttackPoint03");
        attackPointFour = transform.Find("AttackPoint04");
        reloadTimeElapsed = 99f;
    }
    void Update()
    {
        spearDash();
        shieldBlitz();
        swordAction();
        if (currentWeapon != null && currentWeapon.thisWeaponType == weaponData.weaponType.gun)
        {
            reloadTimeElapsed = reloadTimeElapsed += Time.deltaTime;
            if (reloadTimeElapsed > reloadTime)
            {
                canShoot = true;
            }
        }
    }
    public void killPlayer(combatController targetCombat, PlayerController targetController)
    {
        if (!targetCombat.isDead)
        {
            targetCombat.isDead = true;
            targetCombat.StopAllCoroutines();
            targetCombat.player.canMove = false; // and disable their movement.
            roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
            rManager.numOfPlayersAlive--;
            rManager.playerIsDead[targetController.playerID] = true; // Set any player hit as dead...
            rManager.checkForRoundWin();
        }
    }
    public void rayCastHitBox(Transform hitPointTransform, float hitDist)
    {
        RaycastHit ray;
        if (Physics.BoxCast(hitPointTransform.position, new Vector3(.75f, .45f, .75f), transform.forward, out ray, transform.rotation, hitDist, playerLayer))
        {
            goShieldBlitz = false;
            goSwordSlash = false;
            goSpearDash = false;

            // Stop any coroutine related to this function.
            StopCoroutine("swordAttack");
            StopCoroutine("shieldAttack");
            StopCoroutine("spearAttack");

            player.canMove = true;
            combatController enemyCombat = ray.collider.GetComponent<combatController>(); // Fetch the enemy's combatController,
            PlayerController enemyControl = ray.collider.GetComponent<PlayerController>(); // enemy's PlayerController,
            roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
            killPlayer(enemyCombat, enemyControl);
            rManager.checkForRoundWin();
        }
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
                    if (canShoot)
                        StartCoroutine("gunAttack");
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
        rayCastHitBox(attackPointTwo, .5f);
        Debug.DrawRay(attackPointTwo.position, transform.forward, Color.yellow, spearHitRadius);
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
            rayCastHitBox(attackPointOne, spearHitRadius);
            rayCastHitBox(attackPointThree, spearHitRadius);
            rayCastHitBox(attackPointFour, spearHitRadius);
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
            rayCastHitBox(attackPointOne, spearHitRadius);
            rayCastHitBox(attackPointThree, spearHitRadius);
            rayCastHitBox(attackPointFour, spearHitRadius);
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
            rayCastHitBox(attackPointOne, slashDuration);
            rayCastHitBox(attackPointThree, slashDuration);
            rayCastHitBox(attackPointFour, slashDuration);
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
    #region Gun Attack
    public IEnumerator gunAttack()
    {
        canShoot = false;
        player.canMove = false;
        reloadTimeElapsed = 0;
        yield return new WaitForSeconds(shotDuration / 2);
        fireShot();
        shotsLeft = Mathf.Clamp(shotsLeft -= 1, 0, maxShots);
        reloadTimeElapsed = 0;
        yield return new WaitForSeconds(shotDuration / 2);
        reloadTimeElapsed = 0;
        player.canMove = true;
        if (shotsLeft == 0)
        {
            currentWeaponUsable = false;
        }
    }
    public void fireShot()
    {
        GameObject bullet = Instantiate(bulletPrefab, attackPointOne.position, transform.rotation);
        projectile proj = bullet.GetComponent<projectile>();
        proj.owner = player;
        proj.speed = shotPower;
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
                shotsLeft = maxShots;
                canShoot = true;
                gun.SetActive(true);
                break;
        }
    }
    public void unEquipWeapon()
    {
        spear.SetActive(false);
        shield.SetActive(false);
        sword.SetActive(false);
        gun.SetActive(false);
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