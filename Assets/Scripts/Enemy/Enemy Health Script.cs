using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    public float difficulty;

    public float maxHP;
    public float health;

    public float defence;

    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        //Scales enemy health with difficulty
        maxHP *= difficulty;

        //Sets the enemy health
        health = maxHP;
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
        else if (health < 0)
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

    public void TakeDamage(float dmg)
    {
        health -= dmg - defence;
    }
}
