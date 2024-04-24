using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControlScript : MonoBehaviour
{
    [Header("Player Objects")]
    //thomas, ada, kris, eliana 
    public GameObject[] players = { };
    public GameObject player;
    public char chosenPlayer;
    public Stats stats;

    [Header("Movement")]
    public NavMeshAgent agent;
    Vector2 movementInput;

    [Header("IFrames")]
    public bool hasIFrames;
    public float iFramesDuration;
    public float iFramesCooldown;
    public bool canHaveIFrames;

    [Header("Stats")]
    public float maxHP;
    public float currentHP;
    public float defence;
    public float attack;

    [Header("Misc")]
    public bool parryState;
    public bool hasBeenHit;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null && chosenPlayer != '\0')
        {
            GetPlayer();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            CheckStats();

            CheckHealth();

            movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        else if (player == null && chosenPlayer != '\0')
        {
            GetPlayer();
        }

    }

    private void FixedUpdate()
    {
        if (agent.enabled)
        {
            if (Mathf.Abs(movementInput.y) > 0.01f)
            {
                Move(movementInput);
            }
            else
            {
                ROTATE(movementInput);
            }
        }
        
    }

    public void CheckStats()
    {
        if (chosenPlayer == 'T')
        {
            stats = player.GetComponent<ThomasCombatScript>().stats;
            maxHP = stats.maxHealth + stats.maxHealthBonus + (stats.maxHealth * stats.maxHealthPercentMod);
            attack = stats.attack + stats.attackBonus + (stats.attack * stats.attackPercentMod);
            defence = stats.defence + stats.defenceBonus + (stats.defence * stats.defencePercentMod);
        }
        if (chosenPlayer == 'A')
        {
            stats = player.GetComponent<AdaCombatScript>().stats;
            maxHP = stats.maxHealth + stats.maxHealthBonus + (stats.maxHealth * stats.maxHealthPercentMod);
            attack = stats.attack + stats.attackBonus + (stats.attack * stats.attackPercentMod);
            defence = stats.defence + stats.defenceBonus + (stats.defence * stats.defencePercentMod);
        }
        if (chosenPlayer == 'K')
        {
            stats = player.GetComponent<KrisCombatScript>().stats;
            maxHP = stats.maxHealth + stats.maxHealthBonus + (stats.maxHealth * stats.maxHealthPercentMod);
            attack = stats.attack + stats.attackBonus + (stats.attack * stats.attackPercentMod);
            defence = stats.defence + stats.defenceBonus + (stats.defence * stats.defencePercentMod);
        }
        if (chosenPlayer == 'E')
        {
            stats = player.GetComponent<ElianaCombatScript>().stats;
            maxHP = stats.maxHealth + stats.maxHealthBonus + (stats.maxHealth * stats.maxHealthPercentMod);
            attack = stats.attack + stats.attackBonus + (stats.attack * stats.attackPercentMod);
            defence = stats.defence + stats.defenceBonus + (stats.defence * stats.defencePercentMod);
        }

    }

    #region Movement
    public void Move(Vector2 input)
    {
        Vector3 destination = transform.position + transform.right * input.x + transform.forward * input.y;
        agent.destination = destination;
    }

    public void ROTATE(Vector2 input)
    {
        agent.destination = transform.position;
        transform.Rotate(0, input.x * agent.angularSpeed * Time.deltaTime, 0);
    }
    #endregion

    #region DefaultStats
    public void ThomasDefaultStats(Stats player)
    {
        player.maxHealth = 300;
        player.maxHealthBonus = 0;
        player.maxHealthPercentMod = 0;

        player.attack = 60;
        player.attackBonus = 0;
        player.attackPercentMod = 0;

        player.defence = 30;
        player.defenceBonus = 0;
        player.defencePercentMod = 0;
    }

    public void AdaDefaultStats(Stats player)
    {
        player.maxHealth = 400;
        player.maxHealthBonus = 0;
        player.maxHealthPercentMod = 0;

        player.attack = 30;
        player.attackBonus = 0;
        player.attackPercentMod = 0;

        player.defence = 40;
        player.defenceBonus = 0;
        player.defencePercentMod = 0;
    }

    public void KrisDefaultStats(Stats player)
    {
        player.maxHealth = 600;
        player.maxHealthBonus = 0;
        player.maxHealthPercentMod = 0;

        player.attack = 0;
        player.attackBonus = 0;
        player.attackPercentMod = 0;

        player.defence = 50;
        player.defenceBonus = 0;
        player.defencePercentMod = 0;
    }

    public void ElianaDefaultStats(Stats player)
    {
        player.maxHealth = 400;
        player.maxHealthBonus = 0;
        player.maxHealthPercentMod = 0;

        player.attack = 70;
        player.attackBonus = 0;
        player.attackPercentMod = 0;

        player.defence = 10;
        player.defenceBonus = 0;
        player.defencePercentMod = 0;
    }
    #endregion

    #region Health
    public void TakeDamage(float dmg)
    {
        if (parryState)
        {
            hasBeenHit = true;
            parryState = false;
        }
        else
        {
            if (!hasIFrames)
            {
                //Calculates incoming damage
                dmg -= defence;

                //Makes sure the damage doesn't heal the player
                if (dmg > 0)
                {
                    currentHP -= dmg;
                }

                if (canHaveIFrames)
                {
                    hasIFrames = true;
                    canHaveIFrames = false;
                    StartCoroutine(RemoveIFrames());
                    Invoke("RemoveIFrameCooldown", iFramesCooldown);
                }
            }
        }
    }

    public void CheckHealth()
    {
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        else if (currentHP <= 0)
        {
            currentHP = 0;
        }

        if (currentHP == 0)
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

    public void GetPlayer()
    {
        agent = GetComponent<NavMeshAgent>();

        if (chosenPlayer == 'T')
        {
            Quaternion r = new Quaternion(0, 0, 0, 0);
            player = Instantiate(players[0], transform.position, r, gameObject.transform);

            ThomasDefaultStats(player.GetComponent<ThomasCombatScript>().stats);
        }

        if (chosenPlayer == 'A')
        {
            Quaternion r = new Quaternion(0, 0, 0, 0);
            player = Instantiate(players[1], transform.position, r, gameObject.transform);

            AdaDefaultStats(player.GetComponent<AdaCombatScript>().stats);
        }

        if (chosenPlayer == 'K')
        {
            Quaternion r = new Quaternion(0, 0, 0, 0);
            player = Instantiate(players[2], transform.position, r, gameObject.transform);

            KrisDefaultStats(player.GetComponent<KrisCombatScript>().stats);
        }

        if (chosenPlayer == 'E')
        {
            Quaternion r = new Quaternion(0, 0, 0, 0);
            player = Instantiate(players[3], transform.position, r, gameObject.transform);

            ElianaDefaultStats(player.GetComponent<ElianaCombatScript>().stats);
        }

        CheckStats();

        currentHP = maxHP;
    }

    public IEnumerator RemoveIFrames()
    {
        yield return new WaitForSeconds(iFramesDuration);
        yield return hasIFrames = false;
    }

    public void RemoveIFrameCooldown()
    {
        canHaveIFrames = true;
    }
}
