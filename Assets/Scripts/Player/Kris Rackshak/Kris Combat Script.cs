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
    [HideInInspector] public float defence;

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
                if (hasUltIframes)
                {
                    pCS.hasIFrames = true;
                }

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

        //Makes it so the player is not in the parry state
        pCS.parryState = false;

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

            //Attacks the enemies
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, whatIsEnemy);

            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealthScript>().TakeDamage(parryDMGScale * defence);
            }

            StartCoroutine(ResetCooldown(1, 1));
        }

        if (!(Input.GetKey(KeyCode.Space) || !Input.GetMouseButton(1)))
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
