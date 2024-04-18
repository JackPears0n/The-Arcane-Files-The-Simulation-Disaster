using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyScript : MonoBehaviour
{
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
        }
        else
        {
            agent.destination = transform.position;
            return;
        }
    }

    public IEnumerator Attack()
    {
        attackReady = false;

        if (target.GetComponent<PlayerControlScript>().chosenPlayer == 'T')
        {            
            GameObject p = target.GetComponent<PlayerControlScript>().player;
            p.GetComponent<ThomasCombatScript>().TakeDamage(attack);
        }

        if (target.GetComponent<PlayerControlScript>().chosenPlayer == 'A')
        {
            GameObject p = target.GetComponent<PlayerControlScript>().player;
            p.GetComponent<AdaCombatScript>().TakeDamage(attack);
        }

        if (target.GetComponent<PlayerControlScript>().chosenPlayer == 'K')
        {
            GameObject p = target.GetComponent<PlayerControlScript>().player;
            p.GetComponent<KrisCombatScript>().TakeDamage(attack);
        }

        if (target.GetComponent<PlayerControlScript>().chosenPlayer == 'E')
        {
            GameObject p = target.GetComponent<PlayerControlScript>().player;
            p.GetComponent<ElianaCombatScript>().TakeDamage(attack);
        }

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
