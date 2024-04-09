using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Stats", order = 1)]
public class ThomasStats : ScriptableObject
{
    public float maxHealth;
    public float maxHealthPercentMod;
    public float maxHealthBonus;

    public float attack;
    public float attackPercentMod;
    public float attackBonus;

    public float defence;
    public float defencePercentMod;
    public float defenceBonus;
}
