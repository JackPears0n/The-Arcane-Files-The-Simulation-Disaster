using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Error_Code_Orb_Script : MonoBehaviour
{
    private NavMeshAgent agent;

    public GameObject target;

    public float attackRange;
    public bool playerInAttackRange;

    public float attack;
    public float[] skillDMGScale;
    public GameObject projectile;

    public LayerMask whatIsPlayer;

    public bool[] attackReady;
    public float[] attackCooldowns;

    public GameObject[] parryObjects;

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

        //Does the parry mode functionalty

        if (eHS.hasBeenHit && attackReady[1])
        {
            eHS.isParrying = false;
            attackReady[1] = false;

            target.GetComponent<PlayerControlScript>().TakeDamage(skillDMGScale[1] * attack);
            eHS.health += eHS.maxHP * 0.01f;

            foreach (GameObject g in parryObjects)
            {
                g.SetActive(false);
            }

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

        //Instanciate an orb
        GameObject proj = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        proj.GetComponent<Error_Code_OrbProjScript>().GetInfo(gameObject, target);

        StartCoroutine(ResetCooldown(0, attackCooldowns[0]));
        yield return null;
    }

    public IEnumerator Parry()
    {
        foreach (GameObject g in parryObjects)
        {
            g.SetActive(true);
        }
        yield return eHS.isParrying = true;
    }

    public IEnumerator IndividualSkill()
    {
        attackReady[2] = false;

        target.GetComponent<PlayerControlScript>().TakeDamage(skillDMGScale[2] * attack);

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
