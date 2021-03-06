using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class combatController : MonoBehaviour
{
    [SerializeField] public PlayerController player;
    [SerializeField] CharacterController controller;

    public playerAudioManager audioManager;

    [Header("Weapons")] // Each weapon GameObject.
    public GameObject spear;
    public GameObject shield;
    public GameObject sword;
    public GameObject gun;

    [Header("Combat Variables")]
    [SerializeField] LayerMask playerLayer;
    public bool canAttack;
    public bool isDead;
    public bool isKillable;
    public bool inTutorial;
    public weaponData currentWeapon; // Data for current weapon.
    public Transform hand; // Player hand location.
    public Transform leftHand;
    private bool punching;

    public bool weaponEquipped;
    public bool currentWeaponUsable; // Has the weapons' use been exhausted?

    [Header("Attack Points")]
    public Transform attackPointOne; // Attack point.

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
        reloadTimeElapsed = 99f;
    }
    void Update()
    {
        spearDash();
        shieldBlitz();
        swordAction();
        punchMotion();
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
        if (targetCombat.isDead)
            return;
        targetCombat.isDead = true;
        targetCombat.StopAllCoroutines();
        targetCombat.player.canMove = false; // and disable their movement.
        targetCombat.canAttack = false;
        targetCombat.audioManager.playDeathSound();
        roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
        rManager.numOfPlayersAlive -= 1;
        rManager.playerIsDead[targetController.playerID] = true; // Set any player hit as dead...
        rManager.checkForRoundWin();

    }
    public void rayCastHitBox(Transform hitPointTransform, float hitDist)
    {
        RaycastHit ray;
        if (Physics.BoxCast(hitPointTransform.position, new Vector3(.75f, .45f, .2f), transform.forward, out ray, transform.rotation, hitDist, playerLayer))
        {
            goShieldBlitz = false;
            goSwordSlash = false;
            goSpearDash = false;
            punching = false;
            StopCoroutine("swordAttack");
            StopCoroutine("shieldAttack");
            StopCoroutine("spearAttack");
            StopCoroutine("punch");
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
        if (!player.canMove || !canAttack) // Prevent atacking if movement is disabled.
        {
            return;
        }
        if (context.performed && currentWeaponUsable && currentWeapon != null) // Do a weapon attack when a weapon is equipped.
        {
            audioManager.plaAttackSound();
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
            audioManager.plaAttackSound();
        }
    }
    #region unarmed Combat
    public IEnumerator punch()
    {
        player.canMove = false;
        punching = true;
        yield return new WaitForSeconds(.25f);
        punching = false;
        player.canMove = true;
    }
    private void punchMotion()
    {
        if (punching)
        {
            controller.Move(transform.forward * Time.fixedDeltaTime * thrustPower / 6f);
            rayCastHitBox(attackPointOne, spearHitRadius);
        }
    }
    #endregion
    #region Spear Combat
    private void spearDash()
    {
        if (goSpearDash)
        {
            controller.Move(transform.forward * Time.fixedDeltaTime * thrustPower);
            rayCastHitBox(attackPointOne, spearHitRadius);
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
        //Gizmos.DrawWireSphere(attackPointOne.position, spearHitRadius);
        Gizmos.DrawLine(attackPointOne.position, new Vector3(attackPointOne.position.x, attackPointOne.position.y, attackPointOne.position.z + spearHitRadius));
        Gizmos.color = Color.red;
    }
}