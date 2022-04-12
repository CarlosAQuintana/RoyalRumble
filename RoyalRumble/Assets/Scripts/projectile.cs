using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [Header("References")]
    public PlayerController owner;
    public Rigidbody rb;
    public Transform hitPoint;
    public LayerMask playerLayer;

    [Header("Variables")]
    private float lifetime = 10f;
    public float elaspedLife;
    public float hitRadius = 1;
    public float speed;
    public bool isDanger;
    public bool canBounce;
    [SerializeField] private int maxBounces;
    [SerializeField] private int bouncesLeft;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = GetComponentInParent<Rigidbody>();
        hitPoint = transform.Find("hitPoint");

        isDanger = true;

        bouncesLeft = maxBounces;

        rb.velocity = (transform.forward * Time.fixedDeltaTime * speed * 1.5f);
    }
    void Update()
    {
        liveLife();
        lookForHit();
    }
    private void FixedUpdate()
    {

    }
    public void lookForHit()
    {
        if (isDanger)
        {
            RaycastHit ray;
            if (Physics.Raycast(hitPoint.position, transform.forward, out ray, hitRadius, playerLayer))
            {
                isDanger = false;
                combatController enemyCombat = ray.collider.GetComponent<combatController>(); // Fetch the enemy's combatController,
                PlayerController enemyControl = ray.collider.GetComponent<PlayerController>(); // enemy's PlayerController,
                roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
                rManager.numOfPlayersAlive -= 1;
                rManager.playerIsDead[enemyControl.playerID] = true; // Set any player hit as dead...
                enemyCombat.player.canMove = false; // and disable their movement.
                rManager.checkForRoundWin();
                //enemyCombat.isDead = true;
                Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Environment" && !canBounce)
        {
            // Zero our speed, disable dynamic physics, and set threat to zero.
            rb.velocity = (Vector3.zero);
            isDanger = false;
            rb.isKinematic = true;
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Environment") && canBounce)
        {
            Debug.Log("Wall!");
            if (bouncesLeft == 0)
            {
                rb.velocity = (Vector3.zero);
                isDanger = false;
                rb.isKinematic = true;
            }
            bouncesLeft = Mathf.Clamp(bouncesLeft -= 1, 0, maxBounces);
        }
    }
    public void liveLife()
    {
        elaspedLife += Time.deltaTime;
        if (elaspedLife > lifetime)
            Destroy(this.gameObject);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hitPoint.position, hitRadius);
    }
}
