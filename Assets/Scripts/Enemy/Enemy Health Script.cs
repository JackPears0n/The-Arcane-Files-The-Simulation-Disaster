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
    }

    // Update is called once per frame
    void Update()
    {
        HealthCheck();
    }

    public void HealthCheck()
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
            else
            {
                //Takes damage
                health -= dmg - defence;

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
