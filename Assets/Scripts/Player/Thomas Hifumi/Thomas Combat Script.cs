using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ThomasCombatScript : MonoBehaviour
{
    public Animator anim;
    public LayerMask whatIsEnemy;
    public int balence;

    [Header("Stats")]
    public Stats stats;
    [HideInInspector]public float attack;
    public float health;
    public float maxHealth;
    private float defence;
    public float iFramesDuration;
    public bool hasIFrames;

    public float[] cooldowns = { };
    public bool[] cooldownDone = { true, true, true, true };

    [Header("Basic Attack 1")]
    public float bAttack1DMGScale;
    public float bAttack1Range;

    [Header("Basic Attack 2")]
    public float bAttack2DMGScale;
    public GameObject projectile;
    public GameObject projectileLaunchPos; 

    [Header("Individual Skill")]
    public float iSkillHPRegen;

    public bool iSkillG = false;
    public bool iSkillD = false;

    public float skillDuration;

    [Header("Ultimate")]
    public float ultDMGScale;

    // Start is called before the first frame update
    void Start()
    {
        CheckStats();
        health = stats.maxHealth + (stats.maxHealth * (stats.maxHealthPercentMod / 100) + stats.maxHealthBonus);
    }

    // Update is called once per frame
    void Update()
    {
        //Updates the player's stats
        CheckStats();

        CheckHealth();

        //Stops balence going out of bounds
        if (balence < -20)
        {
            balence = -20;
        }
        else if (balence > 20)
        {
            balence = 20;
        }

        //Gets attack input
        AttackInput();
    }

    #region Skills
    public IEnumerator MeleeAttack()
    {
        cooldownDone[0] = false;
        //Two hit AoE attack to all enemies near the player
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, bAttack1Range, whatIsEnemy);

        foreach (Collider enemy in hitEnemies)
        {
            //First attack
            enemy.GetComponent<EnemyHealthScript>().TakeDamage(bAttack1DMGScale * attack);
            if(iSkillD)
            {
                health += (bAttack1DMGScale * attack) / 2;
            }

            //Delays the 2nd attack
            yield return new WaitForSeconds(0.1f);

            //Second attack
            enemy.GetComponent<EnemyHealthScript>().TakeDamage(bAttack1DMGScale * attack);
            if (iSkillD)
            {
                health += (bAttack1DMGScale * attack) / 2;
            }
        }

        //Subtract 1 from balence
        balence--;

        StartCoroutine(ResetCooldown(0, 0));
        yield return null;
    }

    public IEnumerator RangedAttack()
    {
        cooldownDone[1] = false;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 100, whatIsEnemy);
        Collider chosenEnemy = hitEnemies[0];

        //Instanciate an orb
        GameObject proj = Instantiate(projectile, new Vector3 (projectileLaunchPos.transform.position.x, projectileLaunchPos.transform.position.y, projectileLaunchPos.transform.position.z), transform.rotation);
        proj.GetComponent<ThomasProjectileScript>().GetInfo(gameObject, chosenEnemy.gameObject);

        //Make it move towrds the enemy
        //proj.transform.LookAt(chosenEnemy.transform.position);
        //proj.transform.position = Vector3.MoveTowards(proj.transform.position, chosenEnemy.transform.position, 20);


        //Add 1 to the balence
        balence++;

        StartCoroutine(ResetCooldown(1, 1));
        yield return null;
    }

    public IEnumerator IndividualSkill()
    {
        cooldownDone[2] = false;

        //Check for balence
        //If neuteral heal the player once
        if (balence == 0)
        {
            health += 100;
            iSkillD = false;
            iSkillG = false;
        }
        //If negative heal player for the amount of damage dealt from each attack
        else if (balence < 0)
        {
            iSkillD = true;
            iSkillG = false;
            StartCoroutine(RemoveSkillBuff("D"));

        }
        //If postative heal the player for a small amount each time they're hit
        else if (balence < 0)
        {
            iSkillD = false;
            iSkillG = true;
            StartCoroutine(RemoveSkillBuff("G"));

        }

        StartCoroutine(ResetCooldown(2, 2));
        yield return null;
    }

    public IEnumerator Ultimate()
    {
        cooldownDone[3] = false;

        //Inverses the current balence
        if (balence > 0)
        {
            balence *= -1;
        }
        else if (balence < 0)
        {
            balence *= -1;
        }

        //Deals AoE damage to all enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, 100, whatIsEnemy);
        foreach (Collider e in enemies)
        {
            e.gameObject.GetComponent<EnemyHealthScript>().TakeDamage(ultDMGScale * attack);
            if (iSkillD)
            {
                health += (ultDMGScale * attack) / 2;
            }
        }

        StartCoroutine(ResetCooldown(3, 3));
        
        yield return null;
    }
    #endregion

    public void CheckStats()
    {
        maxHealth = stats.maxHealth + (stats.maxHealth * (stats.maxHealthPercentMod / 100) + stats.maxHealthBonus);
        attack = stats.attack + (stats.attack * (stats.attackPercentMod / 100) + stats.attackBonus);
        defence = stats.defence + (stats.defence * (stats.defencePercentMod / 100) + stats.defenceBonus);
    }

    #region Calldowns
    public IEnumerator ResetCooldown(int skillNum, int skillCD)
    {      
        yield return new WaitForSeconds(skillCD);

        yield return cooldownDone[skillNum] = true;

    }

    public IEnumerator RemoveSkillBuff(string buffType)
    {
        yield return new WaitForSeconds(skillDuration);

        if (buffType == "G")
        {
            yield return iSkillG = false;

        }

        if (buffType == "D")
        {
            yield return iSkillD = false;

        }
    }

    public void RemoveIFrames()
    {
       hasIFrames = false;
    }
    /*
    public IEnumerator ResetCooldown(int skillNum, int skillCD)
    {
        yield return new WaitForSeconds(skillCD);

        cooldownDone[skillNum] = true;

        yield return null;

    }

    public IEnumerator RemoveSkillBuff()
    {
        yield return new WaitForSeconds(skillDuration);

        iSkillG = false;
        iSkillD = false;

        yield return null;
    }*/
    #endregion

    public void AttackInput()
    {
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(0)) && cooldownDone[0])
        {
            StartCoroutine(MeleeAttack());
        }

        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(1)) && cooldownDone[1])
        {
            StartCoroutine(RangedAttack());
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
        if (!hasIFrames)
        {
            health -= (dmg - defence);
            hasIFrames = true;
            Invoke(nameof(RemoveIFrames), iFramesDuration);
        }
        else
        {
            return;
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
