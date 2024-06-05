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
    private float nextClickTime = 0f;
    public float lastAttackTime;
    public int noOfClicks;

    [Header("Dodge")]
    private Vector3 dodgeLocation;

    [Header("Individual Skill")]
    public float iSkillDMGScale;

    [Header("Ultimate")]
    public ParticleSystem[] ultPart = { null, null, null, null};
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
        anim = pCS.anim;

        pCS.skillActive[0] = cooldownDone[0];
        pCS.skillActive[1] = cooldownDone[1];
        pCS.skillActive[2] = cooldownDone[2];
        pCS.skillActive[3] = cooldownDone[3];

        if (!pCS.gm.paused)
        {
            if (!pCS.gm.logicPaused)
            {
                CheckStats();

                AttackInput();
            }
        }

        //Resets the noOfClicks if QTE expires
        if (Time.time - lastAttackTime > maxTimeBetweenAttacks)
        {
            noOfClicks = 0;
        }

        #region Anim Manager

        //Stops the anim breaking from spam clicks
        if (noOfClicks == 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("BA3"))
        {
            anim.SetBool("BA2", false);
            anim.SetBool("BA3", false);
        }


        //Auto cancels the animation and makes it go to idle
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("BA1"))
        {
            anim.SetBool("BA1", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("BA2"))
        {
            anim.SetBool("BA2", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("BA3"))
        {
            anim.SetBool("BA3", false);
            noOfClicks = 0;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && anim.GetCurrentAnimatorStateInfo(0).IsName("DP"))
        {
            anim.SetBool("DP", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8 && anim.GetCurrentAnimatorStateInfo(0).IsName("IS"))
        {
            anim.SetBool("IS", false);
        }

        #endregion

        if (ultBuffActive)
        {
            foreach (ParticleSystem p in ultPart)
            {
                p.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (ParticleSystem p in ultPart)
            {
                p.gameObject.SetActive(false);
            }
        }

    }

    #region Skills
    public IEnumerator BasicAttack()
    {
        //Puts skill on cooldown
        cooldownDone[0] = false;

        //Makes sure that the combo attack cancels if input takes too long
       
        if (Time.time > nextClickTime)
        {
            //Logs which sequence muber it is
            lastAttackTime = Time.time;          

            //Animates the 1st attack
            if (noOfClicks == 0)
            {
                //play the 1st animation
                anim.SetBool("BA1", true);
                noOfClicks++;
            }
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 2);

            //Animates the 2nd attack
            if (noOfClicks == 1 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7 /*Makes sure anim is 70% done*/ && anim.GetCurrentAnimatorStateInfo(0).IsName("BA1") /*Name of previous anim*/)
            {
                //play the 2nd animation
                anim.SetBool("BA1", false);
                anim.SetBool("BA2", true);
                noOfClicks++;
            }

            //Animates the 3rd attack
            if (noOfClicks == 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7 /*Makes sure anim is 70% done*/ && anim.GetCurrentAnimatorStateInfo(0).IsName("BA2") /*Name of previous anim*/)
            {
                //play the 3rd animation
                anim.SetBool("BA2", false);
                anim.SetBool("BA3", true);
            }
        }

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

        //Activates the animation
        noOfClicks = 0;
        anim.SetBool("Running", false);
        anim.SetBool("BA1", false);
        anim.SetBool("BA2", false);
        anim.SetBool("BA3", false);
        anim.SetBool("DP", true);
        anim.SetBool("IS", false);

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

        //Activates the animation
        noOfClicks = 0;
        anim.SetBool("Running", false);
        anim.SetBool("BA1", false);
        anim.SetBool("BA2", false);
        anim.SetBool("BA3", false);
        anim.SetBool("DP", false);
        anim.SetBool("IS", true);

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
        StartCoroutine(ResetCooldown(3, 3));
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
