using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrisCombatScript : MonoBehaviour
{
    public Animator anim;
    public LayerMask whatIsEnemy;
    public GameObject player;
    public PlayerControlScript pCS;

    [Header("Stats")]
    public Stats stats;
    public float defence;

    public float attackRange;
    public GameObject attackPoint;

    public float[] cooldowns = { };
    public bool[] cooldownDone = { true, true, true, true };

    [Header("Basic Attack")]
    public float bAttackDMGScale;

    [Header("Parry")]
    public float parryDMGScale;

    [Header("Individual Skill")]
    public float iSkillHPBuff;
    public float iSkillDefBuff;
    public float iSkillDuration;
    public bool isISkillActive;

    [Header("Ultimate")]
    public float ultHeal;
    public float ultIFramesDuration;
    public bool hasUltIframes;
    public ParticleSystem ps;

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

        defence = pCS.defence;
        if (!pCS.gm.paused)
        {
            if (!pCS.gm.logicPaused)
            {
                if (hasUltIframes)
                {
                    pCS.hasIFrames = true;
                }

                CheckStats();

                AttackInput();
            }
        }

        //Enables Ult Particles
        if (hasUltIframes)
        {
            ps.gameObject.SetActive(true);
        }
        else
        {
            ps.gameObject.SetActive(false);
        }

        #region Anim Manager
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("BA"))
        {
            anim.SetBool("BA", false);
        }

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

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("IS"))
        {
            anim.SetBool("IS", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("Ult"))
        {
            anim.SetBool("Ult", false);
        }
        #endregion

    }

    #region Skills
    public IEnumerator BasicAttack()
    {
        //Puts skill on cooldown
        cooldownDone[0] = false;

        //Makes it so the player is not in the parry state
        pCS.parryState = false;

        //Plays the anim
        anim.SetBool("BA", true);
        anim.SetBool("DP", false);
        anim.SetBool("Retaliate", false);
        anim.SetBool("IS", false);
        anim.SetBool("Ult", false);


        //Attacks the enemies
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, whatIsEnemy);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealthScript>().TakeDamage(bAttackDMGScale * defence);
        }

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

        //Makes it so the player is not in the parry state
        pCS.parryState = false;

        //Plays the anim
        anim.SetBool("BA", false);
        anim.SetBool("DP", false);
        anim.SetBool("Retaliate", false);
        anim.SetBool("IS", true);
        anim.SetBool("Ult", false);

        pCS.maxHP += iSkillHPBuff;
        defence += iSkillDefBuff;

        Invoke(nameof(RemoveISBuff), iSkillDuration);
        StartCoroutine(ResetCooldown(2, 2));
        yield return null;
    }
    public IEnumerator Ultimate()
    {
        //Puts skill on cooldown
        cooldownDone[3] = false;

        //Makes it so the player is not in the parry state
        pCS.parryState = false;

        //Plays the anim
        anim.SetBool("BA", false);
        anim.SetBool("DP", false);
        anim.SetBool("Retaliate", false);
        anim.SetBool("IS", false);
        anim.SetBool("Ult", true);

        //Gives the player Ultimate IFrames
        hasUltIframes = true;

        //Heals the player
        pCS.currentHP += ultHeal;

        StartCoroutine(ResetCooldown(3, 3));
        Invoke(nameof(RemoveUltIFrames), ultIFramesDuration);
        yield return null;
    }
    #endregion

    public void CheckStats()
    {
        pCS.defence = stats.defence + (stats.defence * (stats.defencePercentMod / 100) + stats.defenceBonus);
    }

    #region Calldowns
    public IEnumerator ResetCooldown(int skillNum, int skillCD)
    {
        yield return new WaitForSeconds(skillCD);

        yield return cooldownDone[skillNum] = true;

    }
    public void RemoveISBuff()
    {
        pCS.maxHP -= iSkillHPBuff;
        defence -= iSkillDefBuff;
    }
    public void RemoveUltIFrames()
    {
        hasUltIframes = false;
        pCS.hasIFrames = false;
    }
    #endregion

    public void AttackInput()
    {
        if (pCS.hasBeenHit)
        {
            print("Retaliate");
            //Puts skill on cooldown
            cooldownDone[1] = false;

            pCS.parryState = false;
            pCS.hasBeenHit = false;

            //Plays the anim
            anim.SetBool("BA", false);
            anim.SetBool("DP", false);
            anim.SetBool("Retaliate", true);
            anim.SetBool("IS", false);
            anim.SetBool("Ult", false);

            //Attacks the enemies
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, whatIsEnemy);

            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealthScript>().TakeDamage(parryDMGScale * defence);
            }

            StartCoroutine(ResetCooldown(1, 1));
        }

        //Removes parry state if the parry inputs are not being used
        if (!(Input.GetKey(KeyCode.Space) && !Input.GetMouseButton(1)))
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
