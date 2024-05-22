using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

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
    private Rigidbody rb;

    [Header("IFrames")]
    public bool hasIFrames;
    public float iFramesDuration;
    public float iFramesCooldown;
    public bool canHaveIFrames;

    [Header("Stats")]
    public float maxHP;
    public float currentHP;
    public float maxHPPer;
    [HideInInspector] public float maxHPBon;

    public float defence;
    [HideInInspector] public float defPer;
    [HideInInspector] public float defBon;

    public float attack;
    [HideInInspector] public float attkPer;
    [HideInInspector] public float attkBon;

    public bool playerIsDead = false;

    [Header("UI")]
    public Slider hpBar;
    public TMP_Text healthText;

    public GameObject defeatUI;

    [Header("Misc")]
    public bool parryState;
    public bool hasBeenHit;
    public int tokens;

    public GameObject gameManager;
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

        rb = gameObject.GetComponent<Rigidbody>();

        gameManager = GameObject.Find("GM");
        gm = gameManager.GetComponent<GameManager>();

        if (player == null && chosenPlayer != '\0')
        {
            GetPlayer();
        }

        CheckStats();

        if (!gm.paused)
        {
            if (!gm.logicPaused)
            {
                if (player != null)
                {
                    CheckHealth();

                    movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                }
                else if (player == null && chosenPlayer != '\0')
                {
                    GetPlayer();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (gm != null)
        {
            if (!gm.paused)
            {
                if (!gm.logicPaused)
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
            }

        }

        
    }

    public void CheckStats()
    {
        if (chosenPlayer == 'T')
        {
            stats = player.GetComponent<ThomasCombatScript>().stats;
            maxHP = stats.maxHealth + stats.maxHealthBonus + (stats.maxHealth * (stats.maxHealthPercentMod / 100));
            maxHPPer = stats.maxHealthPercentMod;
            maxHPBon = stats.maxHealthBonus;

            attack = stats.attack + stats.attackBonus + (stats.attack * (stats.attackPercentMod / 100));
            attkPer = stats.attackPercentMod;
            attkBon = stats.attackBonus;

            defence = stats.defence + stats.defenceBonus + (stats.defence * (stats.defencePercentMod/100));
            defPer = stats.defencePercentMod;
            defBon = stats.defenceBonus;

        }
        if (chosenPlayer == 'A')
        {
            stats = player.GetComponent<AdaCombatScript>().stats;
            maxHP = stats.maxHealth + stats.maxHealthBonus + (stats.maxHealth * (stats.maxHealthPercentMod / 100));
            maxHPPer = stats.maxHealthPercentMod;
            maxHPBon = stats.maxHealthBonus;

            attack = stats.attack + stats.attackBonus + (stats.attack * (stats.attackPercentMod / 100));
            attkPer = stats.attackPercentMod;
            attkBon = stats.attackBonus;

            defence = stats.defence + stats.defenceBonus + (stats.defence * (stats.defencePercentMod / 100));
            defPer = stats.defencePercentMod;
            defBon = stats.defenceBonus;

        }
        if (chosenPlayer == 'K')
        {
            stats = player.GetComponent<KrisCombatScript>().stats;
            maxHP = stats.maxHealth + stats.maxHealthBonus + (stats.maxHealth * (stats.maxHealthPercentMod / 100));
            maxHPPer = stats.maxHealthPercentMod;
            maxHPBon = stats.maxHealthBonus;

            attack = stats.attack + stats.attackBonus + (stats.attack * (stats.attackPercentMod / 100));
            attkPer = stats.attackPercentMod;
            attkBon = stats.attackBonus;

            defence = stats.defence + stats.defenceBonus + (stats.defence * (stats.defencePercentMod / 100));
            defPer = stats.defencePercentMod;
            defBon = stats.defenceBonus;

        }
        if (chosenPlayer == 'E')
        {
            stats = player.GetComponent<ElianaCombatScript>().stats;
            maxHP = stats.maxHealth + stats.maxHealthBonus + (stats.maxHealth * (stats.maxHealthPercentMod / 100));
            maxHPPer = stats.maxHealthPercentMod;
            maxHPBon = stats.maxHealthBonus;

            attack = stats.attack + stats.attackBonus + (stats.attack * (stats.attackPercentMod / 100));
            attkPer = stats.attackPercentMod;
            attkBon = stats.attackBonus;

            defence = stats.defence + stats.defenceBonus + (stats.defence * (stats.defencePercentMod / 100));
            defPer = stats.defencePercentMod;
            defBon = stats.defenceBonus;

        }

        gm.stats = stats;

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
        healthText.text = "Health: " + currentHP.ToString() + '/' + maxHP.ToString();
        hpBar.value = currentHP;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        else if (currentHP <= 0)
        {
            currentHP = 0;

            if (!playerIsDead)
            {
                StartCoroutine(PlayerDeath());
            }
        }
    }

    public IEnumerator PlayerDeath()
    {
        playerIsDead = true;

        gm.PlayerHasDied();

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

    public void EnableRB()
    {
        //Safely disables the NavMesh
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.enabled = false;

        //Makes the rigidbody non Kinematic
        rb.isKinematic = false;
    }

    public void EnableNavMesh()
    {
        //Makes the rigidbody Kinematic
        rb.isKinematic = true;

        //Safely enables the NavMesh
        agent.enabled = true;
        agent.updateRotation = true;
        agent.updatePosition = true;
        agent.isStopped = false;
    }
}
