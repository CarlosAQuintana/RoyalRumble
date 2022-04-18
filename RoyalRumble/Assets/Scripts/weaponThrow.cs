using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(LineRenderer))]
public class weaponThrow : MonoBehaviour
{
    [Header("Script and Component References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private combatController combat;
    public GameObject aimPointer;

    [Header("Prefabs")]
    [SerializeField] private GameObject spearProjectilePrefab;
    [SerializeField] private GameObject shieldProjectilePrefab;
    [SerializeField] private GameObject gunProjectilePrefab;
    [SerializeField] private GameObject swordProjectilePrefab;

    [Header("Throwing Variables")]
    [SerializeField] private bool throwStarted;
    [SerializeField] private float throwPower;
    [SerializeField] private float throwScale;

    [Header("Shield Specific Variables")]
    public int bounces;
    public float maxLength;
    private LineRenderer lRenderer;
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        lRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
        player = GetComponent<PlayerController>();
        combat = GetComponent<combatController>();
        throwStarted = false;
    }
    void Update()
    {
        shieldBounceRay();
    }
    public void tossWeapon(InputAction.CallbackContext context) // Select a throwing action based on type of weapon.
    {
        if (context.started && combat.currentWeapon != null)
        {
            throwStarted = true;
            if (combat.currentWeapon.thisWeaponType != weaponData.weaponType.shield)
                aimPointer.SetActive(true);
        }
        if (context.canceled && throwStarted && combat.currentWeapon != null)
        {
            throwStarted = false;
            aimPointer.SetActive(false);
            GetComponent<playerAnimController>().anim.SetTrigger("release");
            switch (combat.currentWeapon.thisWeaponType)
            {
                case weaponData.weaponType.spear:
                    spearThrow();
                    break;
                case weaponData.weaponType.shield:
                    shieldthrow();
                    break;
                case weaponData.weaponType.gun:
                    gunThrow();
                    break;
                case weaponData.weaponType.sword:
                    swordThrow();
                    break;
            }
        }
    }
    public void spearThrow() // Spawn the projectile and set it's speed.
    {
        Vector3 modSpawn = new Vector3(combat.attackPointOne.position.x, combat.attackPointOne.position.y, combat.attackPointOne.position.z + 2f);
        GameObject temp = spearProjectilePrefab;
        Instantiate(temp, modSpawn, transform.rotation);
        projectile proj = temp.GetComponent<projectile>();
        proj.owner = player;
        proj.speed = throwPower * throwScale;
        combat.unEquipWeapon();
    }
    public void gunThrow()
    {
        Vector3 modSpawn = new Vector3(combat.attackPointOne.position.x, combat.attackPointOne.position.y, combat.attackPointOne.position.z + 2f);
        GameObject temp = gunProjectilePrefab;
        Instantiate(temp, modSpawn, transform.rotation);
        projectile proj = temp.GetComponent<projectile>();
        proj.owner = player;
        proj.speed = throwPower * throwScale;
        combat.unEquipWeapon();
    }
    public void swordThrow()
    {
        Vector3 modSpawn = new Vector3(combat.attackPointOne.position.x, combat.attackPointOne.position.y, combat.attackPointOne.position.z + 2f);
        GameObject temp = swordProjectilePrefab;
        Instantiate(temp, modSpawn, transform.rotation);
        projectile proj = temp.GetComponent<projectile>();
        proj.owner = player;
        proj.speed = throwPower * throwScale;
        combat.unEquipWeapon();
    }
    public void shieldthrow()
    {
        Vector3 modSpawn = new Vector3(combat.attackPointOne.position.x, combat.attackPointOne.position.y, combat.attackPointOne.position.z + 2f);
        GameObject temp = shieldProjectilePrefab;
        Instantiate(temp, modSpawn, transform.rotation);
        projectile proj = temp.GetComponent<projectile>();
        proj.canBounce = true;
        proj.owner = player;
        proj.speed = throwPower * throwScale;
        combat.unEquipWeapon();
    }
    private void shieldBounceRay()
    {
        if (throwStarted && combat.currentWeapon.thisWeaponType == weaponData.weaponType.shield)
        {
            ray = new Ray(combat.attackPointOne.position, transform.forward);
            lRenderer.positionCount = 1;
            lRenderer.SetPosition(0, transform.position);
            float remainingLength = maxLength;
            for (int i = 0; i < bounces; i++)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
                {
                    lRenderer.positionCount += 1;
                    lRenderer.SetPosition(lRenderer.positionCount - 1, hit.point);
                    remainingLength -= Vector3.Distance(ray.origin, hit.point);
                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                    if (hit.collider.tag != "Environment")
                        break;
                }
                else
                {
                    lRenderer.positionCount += 1;
                    lRenderer.SetPosition(lRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
                }
            }
        }
        else if (!throwStarted)
        {
            lRenderer.positionCount = 0;
        }
    }
}
