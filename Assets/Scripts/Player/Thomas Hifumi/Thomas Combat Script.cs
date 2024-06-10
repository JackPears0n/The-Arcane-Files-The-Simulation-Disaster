using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ThomasCombatScript : MonoBehaviour
{
    public Animator anim;
    public LayerMask whatIsEnemy;
    public int balence;
    public GameObject player;
    public PlayerControlScript pCS;

    [Header("Stats")]
    public Stats stats;
    [HideInInspector] public float attack;

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

    public ParticleSystem[] isPart;

    [Header("Ultimate")]
    public float ultDMGScale;


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
                //Updates the player's stats
                CheckStats();

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
        }

        if (iSkillG)
        {
            isPart[0].gameObject.SetActive(true);
        }
        else
        {
            isPart[0].gameObject.SetActive(false);
        }
        
        if (iSkillD)
        {
            isPart[1].gameObject.SetActive(true);
        }
        else
        {
            isPart[1].gameObject.SetActive(false);
        }

        #region Anim Manager
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("BA1"))
        {
            anim.SetBool("BA1", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && anim.GetCurrentAnimatorStateInfo(0).IsName("BA2"))
        {
            anim.SetBool("BA2", false);
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
    public IEnumerator MeleeAttack()
    {
        cooldownDone[0] = false;
        //Two hit AoE attack to all enemies near the player
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, bAttack1Range, whatIsEnemy);

        //Animate
        anim.SetBool("BA1", true);
        anim.SetBool("BA2", false);
        anim.SetBool("IS", false);
        anim.SetBool("Ult", false);

        foreach (Collider enemy in hitEnemies)
        {
            //First attack
            enemy.GetComponent<EnemyHealthScript>().TakeDamage(bAttack1DMGScale * attack);
            if(iSkillD)
            {
                pCS.currentHP += (bAttack1DMGScale * attack) / 2;
            }

            if (!enemy.GetComponent<EnemyHealthScript>().isDead && enemy != null)
            {
                //Delays the 2nd attack
                yield return new WaitForSeconds(0.1f);

                //Second attack
                enemy.GetComponent<EnemyHealthScript>().TakeDamage(bAttack1DMGScale * attack);
                if (iSkillD)
                {
                    pCS.currentHP += (bAttack1DMGScale * attack) / 2;
                }
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

        //Animate
        anim.SetBool("BA1", false);
        anim.SetBool("BA2", true);
        anim.SetBool("IS", false);
        anim.SetBool("Ult", false);

        if (hitEnemies.Length > 0)
        {
            GameObject chosenEnemy = hitEnemies[0].gameObject;

            //Instanciate an orb
            GameObject proj = Instantiate(projectile, new Vector3(projectileLaunchPos.transform.position.x, projectileLaunchPos.transform.position.y, projectileLaunchPos.transform.position.z), transform.rotation);
            proj.GetComponent<ThomasProjectileScript>().GetInfo(gameObject, chosenEnemy.gameObject);

            //Add 1 to the balence
            balence++;
        }

        StartCoroutine(ResetCooldown(1, 1));
        yield return null;
    }

    public IEnumerator IndividualSkill()
    {
        cooldownDone[2] = false;

        //Animate
        anim.SetBool("BA1", false);
        anim.SetBool("BA2", false);
        anim.SetBool("IS", true);
        anim.SetBool("Ult", false);

        //Check for balence
        //If neuteral heal the player once
        if (balence == 0)
        {
            pCS.currentHP += 100;
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
        else if (balence > 0)
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

        //Animate
        anim.SetBool("BA1", false);
        anim.SetBool("BA2", false);
        anim.SetBool("IS", false);
        anim.SetBool("Ult", true);

        //Inverses the current balence
        if (balence > 0)
        {
            balence *= -1;
        }
        else if (balence < 0)
        {
            balence *= -1;
        }

        stats.attackBonus += 20;

        //Deals AoE damage to all enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, 100, whatIsEnemy);
        foreach (Collider e in enemies)
        {
            e.gameObject.GetComponent<EnemyHealthScript>().TakeDamage(ultDMGScale * attack);
            if (iSkillD)
            {
                pCS.currentHP += (ultDMGScale * attack) / 2;
            }
        }

        StartCoroutine(ResetCooldown(3, 3));
        
        yield return null;
    }
    #endregion

    public void CheckStats()
    {
        attack = stats.attack + (stats.attack * (stats.attackPercentMod / 100) + stats.attackBonus);
    }

    #region Calldowns
    public IEnumerator ResetCooldown(int skillNum, int skillCD)
    {   
        if (skillNum == 3)
        {
            stats.attackBonus -= 20;
        }
        
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
}
