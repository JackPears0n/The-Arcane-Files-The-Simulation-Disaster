using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Study_Gaunt_General : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Player")]
    public GameObject target;
    public bool playerInAttackRange;
    public LayerMask whatIsPlayer;
    
    [Header("Stats")]
    public EnemyHealthScript eHS;
    public float attack;
    public int bossPhase;
    public float attackRange;
    public float[] skillDMGScale;

    [Header("Cooldowns")]
    public bool[] attackReady;
    public float[] attackCooldowns;

    [Header("IS")]
    public int iSkillATKBuff;

    [Header("Ult")]
    public int UltIFrameDuration;
    public GameObject husk;
    public GameObject[] summonedEntities = { null, null, null };

    // Start is called before the first frame update
    void Start()
    {
        float difficulty = gameObject.GetComponent<EnemyHealthScript>().difficulty;
        attack *= difficulty;

        eHS = gameObject.GetComponent<EnemyHealthScript>();

        target = GameObject.Find("Player Object");
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        bossPhase = eHS.bossPhase;

        if (bossPhase == 0)
        {
            attack = 150;
            StartCoroutine(Phase1Brain());
        }

        if (bossPhase == 1)
        {
            attack = 175;
            StartCoroutine(Phase2Brain());
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

    public IEnumerator Phase1Brain()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //Enters the parrying mode
        if (!eHS.isParrying && attackReady[1] && playerInAttackRange)
        {
            eHS.isParrying = true;

            gameObject.GetComponent<EnemyHealthScript>().hasIFrames = true;
            StartCoroutine(gameObject.GetComponent<EnemyHealthScript>().EnableIFrames());
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
            else if (eHS.isParrying && attackReady[1])
            {
                StartCoroutine(Parry());
            }
        }

        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator Phase2Brain()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //Triggers parry on hit
        if (eHS.hasBeenHit)
        {
            StartCoroutine(Parry());
        }

        //Enters the parrying mode
        if (!eHS.isParrying && attackReady[1] && playerInAttackRange)
        {
            eHS.isParrying = true;
            eHS.isParrying = true;

            gameObject.GetComponent<EnemyHealthScript>().hasIFrames = true;
            StartCoroutine(gameObject.GetComponent<EnemyHealthScript>().EnableIFrames());
        }

        if (playerInAttackRange)
        {
            if (attackReady[3] && !eHS.isParrying)
            {
                StartCoroutine(Ultimate());
            }
            //Uses the ISkill
            else if (attackReady[2] && !eHS.isParrying)
            {
                StartCoroutine(IndividualSkill());
            }
            //Uses the BA
            else if (attackReady[0] && !eHS.isParrying)
            {
                StartCoroutine(BasicAttack());
            }
            //Does the parry mode functionalty
            else if (eHS.isParrying && attackReady[1])
            {
                StartCoroutine(Parry());
            }
        }

        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator BasicAttack()
    {
        attackReady[0] = false;

        eHS.isParrying = false;

        target.GetComponent<PlayerControlScript>().TakeDamage(skillDMGScale[0] * attack);

        StartCoroutine(ResetCooldown(0, attackCooldowns[0]));
        yield return null;
    }

    public IEnumerator Parry()
    {
        attackReady[1] = false;

        eHS.hasBeenHit = false;
        eHS.isParrying = false;
        eHS.hasIFrames = false;

        eHS.health += eHS.maxHP * 0.1f;
        

        StartCoroutine(ResetCooldown(1, attackCooldowns[1]));
        yield return null;
    }

    public IEnumerator IndividualSkill()
    {
        attackReady[2] = false;

        eHS.isParrying = false;

        attack += iSkillATKBuff;

        StartCoroutine(ResetCooldown(2, 2));
        yield return null;
    }

    public IEnumerator Ultimate()
    {
        attackReady[3] = false;

        eHS.isParrying = false;

        eHS.hasIFrames = true;
        eHS.canHaveIFrames = false;

        foreach (GameObject e in summonedEntities)
        {
            int enemyNo = 0;

            if (summonedEntities[enemyNo] == null && enemyNo < 3)
            {
                summonedEntities[enemyNo] = Instantiate(husk, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                summonedEntities[enemyNo].GetComponent<BasicEnemyScript>().attack += 20;
                enemyNo++;
            }

            if (enemyNo >= 2)
            {
                enemyNo = 0;
            }

        }

        StartCoroutine(ResetCooldown(3, 3));
        yield return null;
    }

    public IEnumerator ResetCooldown(int skillNO, float cooldownLength)
    {
        if (skillNO == 1 && cooldownLength == 1)
        {
            eHS.isParrying = false;
        }

        if (skillNO == 2 && cooldownLength == 2)
        {
            attack -= iSkillATKBuff;
        }

        if (skillNO == 3 && cooldownLength == 3)
        {
            eHS.Invoke("EnableIFrames", UltIFrameDuration);
            eHS.Invoke("RemoveIFrameCooldown", eHS.iFrameCooldown);
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
