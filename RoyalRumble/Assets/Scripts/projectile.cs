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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hitPoint = transform.Find("hitPoint");
        rb.velocity = (transform.forward * speed);
        isDanger = true;
    }
    void Update()
    {
        liveLife();
        lookForHit();
    }
    private void FixedUpdate()
    {
        rb.velocity = (transform.forward * Time.deltaTime * speed);
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
        if (other.tag == "Environment")
        {
            // Zero our speed, disable dynamic physics, and set threat to zero.
            rb.velocity = (Vector3.zero);
            isDanger = false;
            rb.isKinematic = true;
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
