using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyScript : MonoBehaviour
{
    public Animator anim;

    private NavMeshAgent agent;

    public GameObject target;

    public float attackRange;
    public bool playerInAttackRange;

    public float attack;

    public LayerMask whatIsPlayer;

    public bool attackReady;
    public float attackCooldown;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player Object");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        anim = gameObject.GetComponent<EnemyHealthScript>().anim;

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange)
        {
            if (attackReady)
            {
                StartCoroutine(Attack());
            }
        }
    }

    void FixedUpdate()
    {
        if (!playerInAttackRange)
        {
            agent.destination = target.transform.position;
            anim.SetBool("Running", true);
        }
        else
        {
            agent.destination = transform.position;
            anim.SetBool("Running", false);
            return;
        }
    }

    public IEnumerator Attack()
    {
        attackReady = false;

        anim.SetBool("Running", false);
        anim.SetBool("BA", true);

        yield return new WaitForSeconds(1.2f);

        anim.SetBool("BA", false);
        target.GetComponent<PlayerControlScript>().TakeDamage(attack);

        Invoke(nameof(resetBA), attackCooldown);

        yield return null;
    }

    public void resetBA()
    {
        attackReady = true;
    }

    // Used for Debugging the Check Sphere
    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, attackRange);
    }*/

}
