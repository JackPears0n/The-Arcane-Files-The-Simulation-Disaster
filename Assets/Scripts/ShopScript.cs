using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public GameObject player;
    public PlayerControlScript pcs;
    public Stats playerStats;
    public int tokens;
    public PlayerBuffScript pbs;

    public TMP_Text[] text;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player Object");
        pbs = player.GetComponent<PlayerBuffScript>();
    }

    // Update is called once per frame
    void Update()
    {
        tokens = player.GetComponent<PlayerControlScript>().tokens;
        playerStats = player.GetComponent<PlayerControlScript>().stats;
        pcs = player.GetComponent<PlayerControlScript>();

        UpdateUI();
    }

    public void UpdateUI()
    {
        //Tokens
        text[0].text = tokens.ToString();

        //HP
        text[1].text = pcs.maxHP.ToString();
        text[2].text = playerStats.maxHealthPercentMod.ToString();
        text[3].text = playerStats.maxHealthBonus.ToString();

        //Attack
        text[4].text = pcs.attack.ToString();
        text[5].text = playerStats.attackPercentMod.ToString();
        text[6].text = playerStats.attackBonus.ToString();

        //HP
        text[7].text = pcs.defence.ToString();
        text[8].text = playerStats.defencePercentMod.ToString();
        text[9].text = playerStats.defenceBonus.ToString();
    }

    public void AddStat(int statNum)
    {
        if (tokens > 0)
        {
            pbs.AddStat(statNum);
            tokens--;
        }
    }
}
