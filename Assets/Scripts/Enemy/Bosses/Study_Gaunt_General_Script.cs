using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Study_Gaunt_General_Script : MonoBehaviour
{
    private NavMeshAgent agent;

    private Animator anim;

    public GameObject parryShield;

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
    private bool iskillBuffActive;
    public ParticleSystem is_ps;

    [Header("Ult")]
    public int UltIFrameDuration;
    private bool ultBuffActive;
    public GameObject husk;
    public GameObject[] summonedEntities = { null, null, null };
    public ParticleSystem u_ps;

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
        anim = eHS.anim;

        bossPhase = eHS.bossPhase;

        if (bossPhase == 0)
        {
            eHS.maxHP = 6000;
            attack = 150;
            StartCoroutine(Phase1Brain());
        }

        if (bossPhase == 1)
        {
            eHS.maxHP = 4000;
            attack = 175;
            StartCoroutine(Phase2Brain());
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("BA"))
        {
            anim.SetBool("BA", false);
        }

        if (ultBuffActive)
        {
            u_ps.gameObject.SetActive(true);
        }
        else
        {
            u_ps.gameObject.SetActive(false);
        }

        if (iskillBuffActive)
        {
            is_ps.gameObject.SetActive(true);
        }
        else
        {
            is_ps.gameObject.SetActive(false);
        }

        if (eHS.isParrying)
        {
            parryShield.gameObject.SetActive(true);
        }
        else
        {
            parryShield.gameObject.SetActive(false);
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
            StartCoroutine(Parry());
        }

        //Parry
        if (eHS.hasBeenHit && attackReady[1])
        {
            eHS.health += eHS.maxHP * 0.1f;

            attackReady[1] = false;

            eHS.hasBeenHit = false;
            eHS.isParrying = false;

            StartCoroutine(ResetCooldown(1, attackCooldowns[1]));
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
        }

        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator Phase2Brain()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //Triggers parry on hit
        if (!eHS.isParrying && attackReady[1] && playerInAttackRange)
        {
            StartCoroutine(Parry());
        }

        //Parry
        if (eHS.hasBeenHit && attackReady[1])
        {
            eHS.health += eHS.maxHP * 0.1f;

            attackReady[1] = false;

            eHS.hasBeenHit = false;
            eHS.isParrying = false;

            StartCoroutine(ResetCooldown(1, attackCooldowns[1]));
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
        }

        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator BasicAttack()
    {
        attackReady[0] = false;

        eHS.isParrying = false;

        //Animate
        anim.SetBool("Running", true);
        anim.SetBool("BA", false);
        anim.SetBool("IS", false);

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

        eHS.isParrying = false;

        //Animate
        anim.SetBool("Running", false);
        anim.SetBool("BA", false);
        anim.SetBool("IS", true);

        attack += iSkillATKBuff;

        iskillBuffActive = true;

        StartCoroutine(ResetCooldown(2, 2));
        yield return null;
    }

    public IEnumerator Ultimate()
    {
        attackReady[3] = false;

        eHS.isParrying = false;

        eHS.hasIFrames = true;
        eHS.canHaveIFrames = false;

        ultBuffActive = true;

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
            iskillBuffActive = false;
        }

        if (skillNO == 3 && cooldownLength == 3)
        {
            eHS.Invoke("EnableIFrames", UltIFrameDuration);
            eHS.Invoke("RemoveIFrameCooldown", eHS.iFrameCooldown);

            ultBuffActive = false;
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
