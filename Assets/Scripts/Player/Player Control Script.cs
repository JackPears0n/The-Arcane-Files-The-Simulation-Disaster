using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControlScript : MonoBehaviour
{
    //thomas, ada, kris, eliana 
    public GameObject[] players = { };
    public GameObject player;
    public char chosenPlayer;

    public float maxHP;
    public float currentHP;

    private NavMeshAgent agent;

    Vector2 movementInput;

    // Start is called before the first frame update
    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
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
}
