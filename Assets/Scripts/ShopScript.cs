using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public GameObject player;
    public PlayerControlScript pcs;
    public Stats stats;
    public int tokens;
    public PlayerBuffScript pbs;

    public TMP_Text[] tmp;

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
        pcs = player.GetComponent<PlayerControlScript>();
        stats = pcs.stats;

        UpdateUI();
    }

    public void UpdateUI()
    {
        //Tokens
        tmp[0].text = tokens.ToString();

        //HP
        tmp[1].text = pcs.maxHP.ToString();
        tmp[2].text = pcs.maxHPPer.ToString();
        tmp[3].text = pcs.maxHPBon.ToString();

        //Attack
        tmp[4].text = pcs.attack.ToString();
        tmp[5].text = pcs.attkPer.ToString();
        tmp[6].text = pcs.attkBon.ToString();

        //Defence
        tmp[7].text = pcs.defence.ToString();
        tmp[8].text = pcs.defPer.ToString();
        tmp[9].text = pcs.defBon.ToString();
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
