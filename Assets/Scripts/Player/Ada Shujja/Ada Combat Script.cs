using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AdaCombatScript : MonoBehaviour
{
    public Animator anim;
    public LayerMask whatIsEnemy;
    public GameObject player;
    public PlayerControlScript pCS;

    [Header("Stats")]
    public Stats stats;
    [HideInInspector] public float attack;

    public float attackRange;
    public GameObject attackPoint;

    public float[] cooldowns = { };
    public bool[] cooldownDone = { true, true, true, true };

    [Header("Basic Attack")]
    public float bAttackDMGScale;
    public float maxTimeBetweenAttacks;
    public float lastAttackTime;
    public int noOfClicks;

    [Header("Dodge")]
    private Vector3 dodgeLocation;

    [Header("Individual Skill")]
    public float iSkillDMGScale;

    [Header("Ultimate")]
    public float ultDMGBuff;
    public float ultHeal;
    public bool ultBuffActive;
    public float ultBuffDuration;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player Object");
        pCS = player.GetComponent<PlayerControlScript>();
        CheckStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pCS.gm.paused)
        {
            if (!pCS.gm.logicPaused)
            {
                CheckStats();

                AttackInput();
            }
        }

    }

    #region Skills
    public IEnumerator BasicAttack()
    {
        //Puts skill on cooldown
        cooldownDone[0] = false;

        //Makes sure that the combo attack cancels if input takes too long
        if (Time.time - lastAttackTime > maxTimeBetweenAttacks)
        {
            noOfClicks = 0;
        }

        //Logs which sequence muber it is
        lastAttackTime = Time.time;
        noOfClicks++;

        #region Animations
        //Plays the animation
        if (noOfClicks == 1)
        {
            //play the first animation
        }
        //Makes sure that the animator isn't currently animating
        else if (noOfClicks == 2 /*&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1*/)
        {
            //play the 2nd animation
        }
        //Makes sure that the animator isn't currently animating
        else if (noOfClicks == 3 /*&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1*/)
        {
            //play the 2nd animation
        }
        //Makes sure that the animator isn't currently animating
        else if (noOfClicks == 4 /*&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1*/)
        {
            //play the 2nd animation
        }
        //Makes sure that the animator isn't currently animating
        else if (noOfClicks == 5 /*&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1*/)
        {
            //play the 2nd animation
            noOfClicks = 0;
        }
        #endregion

        //Attacks the enemies
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, 1.5f, whatIsEnemy);

        foreach (Collider enemy in hitEnemies)
        {
            if (ultBuffActive)
            {
                enemy.GetComponent<EnemyHealthScript>().TakeDamage((bAttackDMGScale * attack) * ultDMGBuff);
                pCS.currentHP += ultHeal;
            }
            else 
            {
                enemy.GetComponent<EnemyHealthScript>().TakeDamage(bAttackDMGScale * attack);
            }
        }

        StartCoroutine(ResetCooldown(0, 0));
        yield return null;
    }
    public IEnumerator Dodge()
    {
        //Puts skill on cooldown
        cooldownDone[1] = false;

        pCS.hasIFrames = true;

        //Safely Activates the Rigidbody
        pCS.EnableRB();

        //Gives the player a direction and force
        player.GetComponent<Rigidbody>().AddForce(new Vector3(player.transform.forward.x * -1 * 2, player.transform.position.y * -1 * 2, player.transform.forward.z * -1 * 2), ForceMode.Impulse);
        yield return new WaitForSeconds(1);

        //Safely Activates the NavMesh
        pCS.EnableNavMesh();

        StartCoroutine(pCS.RemoveIFrames());
        StartCoroutine(ResetCooldown(1, 1));

        yield return null;
    }
    public IEnumerator IndividualSkill()
    {
        //Puts skill on cooldown
        cooldownDone[2] = false;

        //Attacks the enemies
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, 1.5f, whatIsEnemy);

        foreach (Collider enemy in hitEnemies)
        {
            if (ultBuffActive)
            {
                enemy.GetComponent<EnemyHealthScript>().TakeDamage((iSkillDMGScale * attack) * ultDMGBuff);
                pCS.currentHP += ultHeal;
            }
            else
            {
                enemy.GetComponent<EnemyHealthScript>().TakeDamage(iSkillDMGScale * attack);
            }
        }

        StartCoroutine(ResetCooldown(2, 2));
        yield return null;
    }
    public IEnumerator Ultimate()
    {
        //Puts skill on cooldown
        cooldownDone[3] = false;

        player.GetComponent<PlayerControlScript>().agent.speed = 4.5f;

        Invoke(nameof(RemoveUltBuff), ultBuffDuration);
        yield return ultBuffActive = true;
    }
    #endregion

    public void CheckStats()
    {
        attack = stats.attack + (stats.attack * (stats.attackPercentMod / 100) + stats.attackBonus);
    }

    #region Calldowns
    public IEnumerator ResetCooldown(int skillNum, int skillCD)
    {
        yield return new WaitForSeconds(skillCD);

        yield return cooldownDone[skillNum] = true;

    }

    public void RemoveUltBuff()
    {
        ultBuffActive = false;
        player.GetComponent<PlayerControlScript>().agent.speed = 3.5f;
    }
    #endregion

    public void AttackInput()
    {
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(0)) && cooldownDone[0])
        {
            StartCoroutine(BasicAttack());
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1)) && cooldownDone[1])
        {
            StartCoroutine(Dodge());
        }

        if ((Input.GetKeyDown(KeyCode.E)) && cooldownDone[2])
        {
            StartCoroutine(IndividualSkill());
        }

        if ((Input.GetKeyDown(KeyCode.R)) && cooldownDone[3])
        {
           StartCoroutine(Ultimate());
        }

    }
}
