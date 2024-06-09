using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElianaCombatScript : MonoBehaviour
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

    [Header("Parry")]
    public float parryDMGScale;

    [Header("Individual Skill")]
    public float iSkillDMGScale;

    [Header("Ultimate")]
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

        #region Anim Manager
        #region BA
        //Resets the noOfClicks if QTE expires
        if (Time.time - lastAttackTime > maxTimeBetweenAttacks)
        {
            noOfClicks = 0;
        }

        //Auto cancels the animations and makes them go to idle
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
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("BA4"))
        {
            anim.SetBool("BA4", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("BA5"))
        {
            anim.SetBool("BA5", false);
        }

        //Stops the animatiioon from getting stuck
        if (noOfClicks > 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            noOfClicks = 0;
        }
        #endregion
        #region DP
        if (pCS.parryState)
        {
            anim.SetBool("DP", true);
        }
        if (!pCS.parryState)
        {
            anim.SetBool("DP", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("DP"))
        {
            anim.SetBool("DP", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("Retaliate"))
        {
            anim.SetBool("Retaliate", false);
        }

        #endregion
        //IS
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("IS"))
        {
            anim.SetBool("IS", false);
        }
        #endregion
    }

    #region Skills
    public IEnumerator BasicAttack()
    {
        //Puts skill on cooldown
        cooldownDone[0] = false;

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
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 4);

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
                noOfClicks++;
            }

            //Animates the 4th attack
            if (noOfClicks == 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7 /*Makes sure anim is 70% done*/ && anim.GetCurrentAnimatorStateInfo(0).IsName("BA3") /*Name of previous anim*/)
            {
                //play the 4th animation
                anim.SetBool("BA3", false);
                anim.SetBool("BA4", true);
                noOfClicks++;
            }

            //Animates the 5th attack
            if (noOfClicks == 4 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7 /*Makes sure anim is 70% done*/ && anim.GetCurrentAnimatorStateInfo(0).IsName("BA4") /*Name of previous anim*/)
            {
                //play the 5th animation
                anim.SetBool("BA4", false);
                anim.SetBool("BA5", true);
                noOfClicks++;
            }
        }
       
        if (!ultBuffActive)
        {
            //Attacks the enemies
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, whatIsEnemy);
           
            foreach (Collider enemy in hitEnemies)
            {
                if (enemy != null)
                {
                    enemy.gameObject.GetComponent<EnemyHealthScript>().TakeDamage(bAttackDMGScale * attack);
                }
            }
        }

        if (ultBuffActive)
        {
            //Gets every enemy currenly in the game
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                EnemyHealthScript ehs = enemy.GetComponent<EnemyHealthScript>();

                //Insta kill check
                if (ehs.mobType == 'N' && ehs.health <= (ehs.maxHP * 0.2))
                {
                    if (enemy != null)
                    {
                        ehs.isDead = true;
                    }
                }
                else
                {
                    if (enemy != null)
                    {
                        enemy.GetComponent<EnemyHealthScript>().TakeDamage(bAttackDMGScale * attack);
                    }
                }
            }
        }

        print("BA");
        StartCoroutine(ResetCooldown(0, 0));
        yield return null;
    }
    public IEnumerator Parry()
    {
        //Makes it so the player is in the parry state
        yield return pCS.parryState = true;
    }
    public IEnumerator IndividualSkill()
    {
        //Puts skill on cooldown
        cooldownDone[2] = false;

        //Plays anim
        anim.SetBool("BA1", false);
        anim.SetBool("BA2", false);
        anim.SetBool("BA3", false);
        anim.SetBool("BA4", false);
        anim.SetBool("BA5", false);
        anim.SetBool("DP", false);
        anim.SetBool("Retaliate", false);
        anim.SetBool("IS", true);


        if (ultBuffActive)
        {
            //Gets every enemy currenly in the game
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                EnemyHealthScript ehs = enemy.GetComponent<EnemyHealthScript>();

                //Insta kill check
                if (ehs.mobType == 'N' && ehs.health <= (ehs.maxHP * 0.2))
                {
                    ehs.isDead = true;
                }
                else
                {
                    enemy.GetComponent<EnemyHealthScript>().TakeDamage(iSkillDMGScale * attack);

                    yield return new WaitForSeconds(0.1f);

                    enemy.GetComponent<EnemyHealthScript>().TakeDamage(iSkillDMGScale * attack);
                }
            }
        }
        else
        {
            //Attacks the enemies
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, whatIsEnemy);

            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealthScript>().TakeDamage(iSkillDMGScale * attack);

                yield return new WaitForSeconds(0.1f);

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

        Invoke(nameof(RemoveUltBuff), ultBuffDuration);
        yield return ultBuffActive = true;
    }
    #endregion

    public void CheckStats()
    {
        pCS.attack = stats.attack + (stats.attack * (stats.attackPercentMod / 100) + stats.attackBonus);
    }

    #region Calldowns
    public IEnumerator ResetCooldown(int skillNum, int skillCD)
    {
        yield return new WaitForSeconds(skillCD);

        yield return cooldownDone[skillNum] = true;

    }

    public void RemoveParrySpeedBuff()
    {
        player.gameObject.GetComponent<NavMeshAgent>().speed--;
    }
    public void ResetIFrameCooldown()
    {
        pCS.canHaveIFrames = true;
    }

    public void RemoveIFrames()
    {
        pCS.hasIFrames = false;
    }

    public void RemoveUltBuff()
    {
        ultBuffActive = false;
    }
    #endregion

    public void AttackInput()
    {
        if (pCS.hasBeenHit)
        {
            print("Retaliate");

            //Puts skill on cooldown
            cooldownDone[1] = false;

            //Plays anim
            anim.SetBool("BA1", false);
            anim.SetBool("BA2", false);
            anim.SetBool("BA3", false);
            anim.SetBool("BA4", false);
            anim.SetBool("BA5", false);
            anim.SetBool("DP", false);
            anim.SetBool("Retaliate", true);
            anim.SetBool("IS", false);


            pCS.hasBeenHit = false;

            player.gameObject.GetComponent<NavMeshAgent>().speed++;

            if (ultBuffActive)
            {
                //Gets every enemy currenly in the game
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                foreach (GameObject enemy in enemies)
                {
                    EnemyHealthScript ehs = enemy.GetComponent<EnemyHealthScript>();

                    //Insta kill check
                    if (ehs.mobType == 'N' && ehs.health <= (ehs.maxHP * 0.2))
                    {
                        ehs.isDead = true;
                    }
                    else
                    {
                        enemy.GetComponent<EnemyHealthScript>().TakeDamage(bAttackDMGScale * attack);
                    }
                }
            }
            else
            {
                //Attacks the enemies
                Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, whatIsEnemy);

                foreach (Collider enemy in hitEnemies)
                {
                    enemy.GetComponent<EnemyHealthScript>().TakeDamage(bAttackDMGScale * attack);
                }
            }

            Invoke(nameof(RemoveParrySpeedBuff), 5);
            StartCoroutine(ResetCooldown(1, 1));

        }

        //Removes parry state if the parry inputs are not being used
        if (!(Input.GetKey(KeyCode.Space) && !Input.GetMouseButton(1)))
        {
            pCS.parryState = false;
        }

        if (!Input.GetMouseButton(1) || !Input.GetKey(KeyCode.Space))
        {
            pCS.parryState = false;
        }

        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(0)) && cooldownDone[0])
        {
            StartCoroutine(BasicAttack());
        }

        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(1)) && cooldownDone[1])
        {
            StartCoroutine(Parry());
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
