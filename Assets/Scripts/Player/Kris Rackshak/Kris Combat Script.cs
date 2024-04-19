using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrisCombatScript : MonoBehaviour
{
    public Animator anim;
    public LayerMask whatIsEnemy;
    public GameObject player;

    [Header("Stats")]
    public Stats stats;
    [HideInInspector] public float attack;
    public float health;
    public float maxHealth;
    [HideInInspector] public float defence;

    public float attackRange;
    public GameObject attackPoint;

    public float[] cooldowns = { };
    public bool[] cooldownDone = { true, true, true, true };

    [Header("IFrames")]
    public bool hasIFrames;
    public float iFramesDuration;
    public float iFramesCooldown;
    public bool canHaveIFrames;

    [Header("Basic Attack")]
    public float bAttackDMGScale;

    [Header("Parry")]
    public float parryDMGScale;
    public bool parryState;
    public bool hasBeenHit;

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
        CheckStats();
        health = stats.maxHealth + (stats.maxHealth * (stats.maxHealthPercentMod / 100) + stats.maxHealthBonus);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasUltIframes)
        {
            hasIFrames = true;
        }

        CheckStats();

        CheckHealth();

        AttackInput();

    }

    #region Skills
    public IEnumerator BasicAttack()
    {
        //Puts skill on cooldown
        cooldownDone[0] = false;

        //Makes it so the player is not in the parry state
        parryState = false;

        //Attacks the enemies
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, 1.5f, whatIsEnemy);

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
        parryState = true;

        //Only triggers when hit while in parry state
        if (hasBeenHit)
        {
            //Puts skill on cooldown
            cooldownDone[1] = false;

            hasBeenHit = false;
            //Attacks the enemies
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, 1.5f, whatIsEnemy);

            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealthScript>().TakeDamage(parryDMGScale * defence);               
            }

            StartCoroutine(ResetCooldown(1, 1));
        }

        yield return null;
    }
    public IEnumerator IndividualSkill()
    {
        //Puts skill on cooldown
        cooldownDone[2] = false;

        //Makes it so the player is not in the parry state
        parryState = false;

        maxHealth += iSkillHPBuff;
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
        parryState = false;

        //Gives the player Ultimate IFrames
        hasUltIframes = true;

        //Heals the player
        health += ultHeal;

        StartCoroutine(ResetCooldown(3, 3));
        Invoke(nameof(RemoveUltIFrames), ultIFramesDuration);
        yield return null;
    }
    #endregion

    public void CheckStats()
    {
        maxHealth = stats.maxHealth + (stats.maxHealth * (stats.maxHealthPercentMod / 100) + stats.maxHealthBonus);
        attack = stats.attack + (stats.attack * (stats.attackPercentMod / 100) + stats.attackBonus);
        defence = stats.defence + (stats.defence * (stats.defencePercentMod / 100) + stats.defenceBonus);


        player.GetComponent<PlayerControlScript>().maxHP = maxHealth;
        player.GetComponent<PlayerControlScript>().currentHP = health;
    }

    #region Calldowns
    public IEnumerator ResetCooldown(int skillNum, int skillCD)
    {
        yield return new WaitForSeconds(skillCD);

        yield return cooldownDone[skillNum] = true;

    }

    public void ResetIFrameCooldown()
    {
        canHaveIFrames = true;
    }
    public void RemoveIFrames()
    {
        hasIFrames = false;
    }
    public void RemoveISBuff()
    {
        maxHealth -= iSkillHPBuff;
        defence -= iSkillDefBuff;
    }
    public void RemoveUltIFrames()
    {
        hasUltIframes = false;
        hasIFrames = false;
    }
    #endregion

    public void AttackInput()
    {
        if (hasBeenHit)
        {
            //Puts skill on cooldown
            cooldownDone[1] = false;

            hasBeenHit = false;
            //Attacks the enemies
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, 1.5f, whatIsEnemy);

            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealthScript>().TakeDamage(parryDMGScale * defence);
            }

            StartCoroutine(ResetCooldown(1, 1));
        }

        if (!Input.GetMouseButton(1) || !Input.GetKey(KeyCode.Space))
        {
            parryState = false;
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

    #region Health
    public void TakeDamage(float dmg)
    {
        if (parryState)
        {
            hasBeenHit = true;
        }

        if (!parryState)
        {
            if (!hasIFrames)
            {
                health -= (dmg - defence);
                if (canHaveIFrames)
                {
                    hasIFrames = true;
                    canHaveIFrames = false;
                    Invoke("RemoveIFrames", iFramesDuration);
                    Invoke("ResetIFrameCooldown", iFramesCooldown);
                }
            }
            else
            {
                return;
            }
        }
    }

    public void CheckHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health < 0)
        {
            health = 0;
        }

        if (health == 0)
        {
            PlayerDeath();
        }
    }

    public IEnumerator PlayerDeath()
    {
        Debug.Log("Player is dead");
        yield return null;
    }
    #endregion
}
