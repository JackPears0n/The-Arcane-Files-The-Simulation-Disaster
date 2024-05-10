using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThomasProjectileScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public GameObject thomas;
    private ThomasCombatScript thomasCS;

    public GameObject target;
    private EnemyHealthScript targetHS;

    public LayerMask whatIsEnemy;

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, 100, whatIsEnemy);
            if (enemies.Length > 0)
            {
                target = enemies[0].gameObject;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (!thomasCS.pCS.gm.paused)
        {
            if (!thomasCS.pCS.gm.logicPaused)
            {
                if (target != null)
                {
                    agent.destination = target.transform.position;
                }
            }
            else
            {
                agent.destination = transform.position;
            }
        }
        else
        {
            agent.destination = transform.position;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            targetHS.TakeDamage(thomasCS.bAttack2DMGScale * thomasCS.attack);
            Destroy(gameObject);
        }
    }

    public void GetInfo(GameObject player, GameObject enemy)
    {
        thomas = player;
        target = enemy;

        thomasCS = thomas.GetComponent<ThomasCombatScript>();
        targetHS = target.GetComponent<EnemyHealthScript>();
    }
}
