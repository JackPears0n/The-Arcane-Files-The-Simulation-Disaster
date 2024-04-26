using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    public float difficulty;

    public char mobType;

    public float maxHP;
    public float health;

    public float defence;

    public bool isDead = false;

    //Parry Variables
    public bool isParrying;
    public bool hasBeenHit;

    //Duration and status of enemy IFrames
    public float iFrameDuration;
    public bool hasIFrames;

    //Cooldown variables for the IFrames
    public float iFrameCooldown;
    public bool IFramesOffCooldown;

    //False if the mobType is N
    public bool canHaveIFrames;

    //Phases for bosses
    public int bossPhase;
    public int numberOfPhases;


    // Start is called before the first frame update
    void Start()
    {
        //Scales enemy max health  and defencewith difficulty
        maxHP *= difficulty;
        defence *= difficulty;

        //Sets the enemy health
        health = maxHP;

        if (mobType == 'N')
        {
            canHaveIFrames = false;
        }
        else
        {
            canHaveIFrames = true;
        }

        if (mobType == 'B')
        {
            bossPhase = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HealthCheck();
    }

    public void HealthCheck()
    {
        if (mobType == 'B')
        {
            //Stops healther overflowing max value
            if (health > maxHP)
            {
                health = maxHP;
            }

            //Checks if the enemy's health is less than or equal to 0
            if (health <= 0)
            {
                //Sets HP to 0
                health = 0;
                
                //Checks the boss phase
                //If boss isn't in its final phase, move to next phase
                if (bossPhase < numberOfPhases)
                {
                    health = maxHP;
                    bossPhase++;
                }
                //Kills boss
                else
                {
                    isDead = true;
                }
            }

            //Kills enemy is the isDead variable is true
            if (isDead)
            {
                StartCoroutine(EnemyDie());
            }
        }
        else
        {
            if (health > maxHP)
            {
                health = maxHP;
            }
            else if (health <= 0)
            {
                health = 0;
                isDead = true;
            }

            if (isDead)
            {
                StartCoroutine(EnemyDie());
            }
        }

    }

    public IEnumerator EnemyDie()
    {
        //Give the player currency

        //Play death anim

        //Delays the enemy destruction
        yield return new WaitForSeconds(0.1f);

        //Destroys the enemy gameobject
        Destroy(gameObject);
    }

    public IEnumerator EnableIFrames()
    {
        yield return hasIFrames = false;
        yield return canHaveIFrames = true;
    }

    public IEnumerator RemoveIFrameCooldown()
    {
        yield return IFramesOffCooldown = true;
    }

    public void TakeDamage(float dmg)
    {
        if (isParrying)
        {
            hasBeenHit = true;
        }
        else
        {
            if (hasIFrames)
            {
                return;
            }
            else if (gameObject.name == "Error_Code: The Prisoner")
            {
                if (gameObject.GetComponent<Error_Code_The_Prisoner_Script>().ultIsActive)
                {
                    gameObject.GetComponent<Error_Code_The_Prisoner_Script>().takenHits++;
                }
                else
                {
                    //Calculates incoming damage
                    dmg -= defence;

                    //Makes sure the damage doesn't heal the player
                    if (dmg > 0)
                    {
                        health -= dmg;
                    }

                    //Gives IFrames
                    if (canHaveIFrames && IFramesOffCooldown)
                    {
                        hasIFrames = true;
                        canHaveIFrames = false;

                        Invoke(nameof(EnableIFrames), iFrameDuration);
                        Invoke(nameof(RemoveIFrameCooldown), iFrameCooldown);
                    }
                }
            }
            else
            {
                //Calculates incoming damage
                dmg -= defence;

                //Makes sure the damage doesn't heal the player
                if (dmg > 0)
                {
                    health -= dmg;
                }

                //Gives IFrames
                if (canHaveIFrames && IFramesOffCooldown)
                {
                    hasIFrames = true;
                    canHaveIFrames = false;

                    Invoke(nameof(EnableIFrames), iFrameDuration);
                    Invoke(nameof(RemoveIFrameCooldown), iFrameCooldown);
                }
            }

        }
        
    }
}
