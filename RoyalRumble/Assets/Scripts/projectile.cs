using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public PlayerController owner;
    public Rigidbody rb;
    public Transform hitPoint;
    public LayerMask playerLayer;
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

    // Update is called once per frame
    void Update()
    {
        lookForHit();
    }
    public void lookForHit()
    {
        if (isDanger)
        {
            Collider[] spearCol = Physics.OverlapSphere(hitPoint.position, hitRadius, playerLayer);
            if (spearCol != null)
            {
                Debug.Log("Hit " + spearCol.Length + " players!");
                isDanger = false;
                for (int i = 0; i < spearCol.Length; i++)
                {
                    combatController enemyCombat = spearCol[i].GetComponent<combatController>(); // Fetch the enemy's combatController,
                    PlayerController enemyControl = spearCol[i].GetComponent<PlayerController>(); // enemy's PlayerController,
                    roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
                    rManager.numOfPlayersAlive -= 1;
                    rManager.playerIsDead[enemyControl.playerID] = true; // Set any player hit as dead...
                    enemyCombat.player.canMove = false; // and disable their movement.

                    //enemyCombat.isDead = true;
                }
                roundManager round = FindObjectOfType<roundManager>();
                round.checkForRoundWin();

                Destroy(this.gameObject);
                // Zero our speed, disable dynamic physics, and set threat to zero.
                // rb.velocity = (Vector3.zero);
                // rb.isKinematic = true;
                // isDanger = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Environment")
        {
            // Zero our speed, disable dynamic physics, and set threat to zero.
            rb.velocity = (Vector3.zero);
            rb.isKinematic = true;
            isDanger = false;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hitPoint.position, hitRadius);
    }
}
