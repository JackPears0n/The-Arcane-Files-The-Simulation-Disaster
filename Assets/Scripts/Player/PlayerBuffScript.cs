using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffScript : MonoBehaviour
{
    public GameObject playerObject;
    public PlayerControlScript pcs;
    public Stats stats;

    public int statBaseAmount;
    public int statPercentBonuses;
    public int statBonuses;
    
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("Player Object");
        pcs = playerObject.GetComponent<PlayerControlScript>();

        //stats = pCS.GetComponent<PlayerControlScript>().stats;
    }

    // Update is called once per frame
    void Update()
    {
        stats = pcs.stats;
    }

    /*
     * StatList:
     * 0.HP, 1.HP%, 2.BonusHP, 3.Attk, 4.Attk%, 5.BonusAttk, 6.Def, 7.Def%, 8.BonusDef
     */

    //Adds to a stat
    public void AddStat(int statNum)
    {
        if (statNum == 0)
        {
            stats.maxHealth += statBaseAmount;
        }
        else if (statNum == 1)
        {
            stats.maxHealthPercentMod += statPercentBonuses;
        }
        else if (statNum == 2)
        {
            stats.maxHealthBonus += statBonuses;
        }
        else if (statNum == 3)
        {
            stats.attack += statBaseAmount;
        }
        else if (statNum == 4)
        {
            stats.attackPercentMod += statPercentBonuses;
        }
        else if (statNum == 5)
        {
            stats.attackBonus += statBonuses;
        }
        else if (statNum == 6)
        {
            stats.defence += statBaseAmount;
        }
        else if (statNum == 7)
        {
            stats.defencePercentMod += statPercentBonuses;
        }
        else if (statNum == 8)
        {
            stats.defenceBonus += statBonuses;
        }
    }

    //Lowers from a stat
    public void LowerStat(int statNum)
    {
        if (statNum == 0)
        {
            stats.maxHealth -= statBaseAmount;
        }
        else if (statNum == 1)
        {
            stats.maxHealthPercentMod -= statPercentBonuses;
        }
        else if (statNum == 2)
        {
            stats.maxHealthBonus -= statBonuses;
        }
        else if (statNum == 3)
        {
            stats.attack -= statBaseAmount;
        }
        else if (statNum == 4)
        {
            stats.attackPercentMod -= statPercentBonuses;
        }
        else if (statNum == 5)
        {
            stats.attackBonus -= statBonuses;
        }
        else if (statNum == 6)
        {
            stats.defence -= statBaseAmount;
        }
        else if (statNum == 7)
        {
            stats.defencePercentMod -= statPercentBonuses;
        }
        else if (statNum == 8)
        {
            stats.defenceBonus -= statBonuses;
        }
    }
}
