using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Study_Abyssal_Spider_Queen_Script : MonoBehaviour
{
    private NavMeshAgent agent;

    public GameObject target;

    public float attackRange;
    public bool playerInAttackRange;

    public float attack;
    public float[] skillDMGScale;
    public GameObject summonEntity;

    public LayerMask whatIsPlayer;

    public bool[] attackReady;
    public float[] attackCooldowns;

    public EnemyHealthScript eHS;

    // Start is called before the first frame update
    void Start()
    {
        float difficulty = gameObject.GetComponent<EnemyHealthScript>().difficulty;
        attack *= difficulty;

        eHS = gameObject.GetComponent<EnemyHealthScript>();

        target = GameObject.Find("Player Object");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //Enters the parrying mode
        if (!eHS.isParrying && attackReady[1] && playerInAttackRange)
        {
            StartCoroutine(Parry());
        }

        if (playerInAttackRange)
        {
            //Uses the ISkill
            if (attackReady[2] && !eHS.isParrying)
            {
                StartCoroutine(IndividualSkill());
            }
            //Uses the BA
            else if (attackReady[0] && !eHS.isParrying)
            {
                StartCoroutine(BasicAttack());
            }
            //Does the parry mode functionalty
            else if (eHS.hasBeenHit && attackReady[1])
            {
                attackReady[1] = false;

                if (eHS.hasBeenHit)
                {
                    eHS.hasBeenHit = false;
                    eHS.isParrying = false;

                    GameObject enemy = Instantiate(summonEntity, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                }

                StartCoroutine(ResetCooldown(1, attackCooldowns[1]));
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

    public IEnumerator BasicAttack()
    {
        attackReady[0] = false;

        target.GetComponent<PlayerControlScript>().TakeDamage(skillDMGScale[0] * attack);

        StartCoroutine(ResetCooldown(0, attackCooldowns[0]));
        yield return null;
    }

    public IEnumerator Parry()
    {
        yield return eHS.isParrying = true;
    }

    public IEnumerator IndividualSkill()
    {
        attackReady[2] = false; 

        GameObject[] spiders = { null, null, null };

        foreach (GameObject e in spiders)
        {
            int enemyNo = 0;

            if (e == null)
            {
                spiders[enemyNo] = Instantiate(summonEntity, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                spiders[enemyNo].GetComponent<BasicEnemyScript>().attack += 80;
            }

            if (enemyNo >= 2)
            {
                enemyNo = 0;
            }
            else
            {
                enemyNo++;
            }
        }

        StartCoroutine(ResetCooldown(2, attackCooldowns[2]));
        yield return null;
    }

    public IEnumerator ResetCooldown(int skillNO, float cooldownLength)
    {
        if (skillNO == 1 && cooldownLength == 1)
        {
            eHS.isParrying = false;

        }

        yield return new WaitForSeconds(cooldownLength);

        yield return attackReady[skillNO] = true;
    }

    // Used for Debugging the Check Sphere
    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, attackRange);
    }*/

}
