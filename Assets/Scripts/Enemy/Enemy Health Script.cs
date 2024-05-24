using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthScript : MonoBehaviour
{
    public GameObject player;

    public GameManager gm;

    [Header("Stats")]
    public float difficulty;

    public char mobType;

    public float maxHP;
    public float currentMaxHP;
    public float health;

    public float defence;
    public float currentDef;

    public bool isDead = false;

    [Header ("Combat")]
    //Parry Variables
    public bool isParrying;
    public bool hasBeenHit;

    [Header ("I Frames")]
    //Duration and status of enemy IFrames
    public float iFrameDuration;
    public bool hasIFrames;

    //Cooldown variables for the IFrames
    public float iFrameCooldown;
    public bool IFramesOffCooldown;

    [Header("Enemy Type")]
    //False if the mobType is N
    public bool canHaveIFrames;

    //Phases for bosses
    public int bossPhase;
    public int numberOfPhases;

    [Header("UI")]
    public Slider hpBar;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the player
        player = GameObject.Find("Player Object");

        //Gets the game manager
        gm = GameObject.Find("GM").GetComponent<GameManager>();

        //Scales enemy max health  and defencewith difficulty
        currentMaxHP = maxHP *= difficulty;
        currentDef = defence *= difficulty;

        //Sets the enemy health
        health = currentMaxHP;

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
        hpBar.maxValue = currentMaxHP;
        hpBar.value = health;

        if (mobType == 'B')
        {
            //Stops healther overflowing max value
            if (health > currentMaxHP)
            {
                health = currentMaxHP;
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
                    health = currentMaxHP;
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
            if (health > currentMaxHP)
            {
                health = currentMaxHP;
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
        //Play death anim

        //Delays the enemy destruction
        yield return new WaitForSeconds(0.1f);

        //Gives the player a token
        player.GetComponent<PlayerControlScript>().tokens++;

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
            isParrying = false;
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
                    dmg -= currentDef;

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
                print(dmg);
                dmg = dmg - currentDef;
                print(dmg);

                //Makes sure the damage doesn't heal the enemy
                if (dmg >= 0)
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
