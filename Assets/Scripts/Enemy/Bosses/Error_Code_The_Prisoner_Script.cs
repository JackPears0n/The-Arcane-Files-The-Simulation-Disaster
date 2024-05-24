using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Error_Code_The_Prisoner_Script : MonoBehaviour
{
    public GameObject[] models;

    [Header("Player")]
    public GameObject target;
    public LayerMask whatIsPlayer;

    [Header("Stats")]
    public EnemyHealthScript eHS;
    public float attack;
    /*BA, ABA, IS, Ult*/ public float[] skillDMGScale = { 0, 0, 0, 0};

    [Header("Cooldowns")]
    //BA, IS, Ult
    public bool[] attackReady = { true, true, true };
    public float[] attackCooldowns = { 0, 0, 0 };

    [Header("BA")]
    public int noOFBAs;

    [Header("IS")]
    public GameObject errorCodeOrb;
    public GameObject gauntGeneral;
    public float stunDuration;

    [Header("Ult")]
    public int takenHits;
    public bool ultIsActive;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "Error_Code: The Prisoner";
        float difficulty = gameObject.GetComponent<EnemyHealthScript>().difficulty;
        attack *= difficulty;

        eHS = gameObject.GetComponent<EnemyHealthScript>();

        target = GameObject.Find("Player Object");
    }

    void Update()
    {
        if (eHS.bossPhase == 0)
        {
            models[0].SetActive(true);
            eHS.maxHP = 10000;
            attack = 150;
            StartCoroutine(Phase1Brain());
        }

        if (eHS.bossPhase == 1)
        {
            models[1].SetActive(true);
            eHS.maxHP = 8000;
            attack = 175;
            StartCoroutine(Phase2Brain());
        }

        if (eHS.bossPhase == 2)
        {
            models[2].SetActive(true);
            eHS.maxHP = 6000;
            attack = 200;
            StartCoroutine(Phase3Brain());
        }
    }

    void FixedUpdate()
    {

    }

    public IEnumerator Phase1Brain()
    {
        if (attackReady[1])
        {
            StartCoroutine(IndividualSkill());
        }
        else if (attackReady[0])
        {
            StartCoroutine(BasicAttack());
        }

        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator Phase2Brain()
    {
        if (attackReady[1])
        {
            StartCoroutine(IndividualSkill());
        }
        else if (attackReady[0])
        {
            StartCoroutine(BasicAttack());
        }

        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator Phase3Brain()
    {
        if (attackReady[2])
        {
            StartCoroutine(Ultimate());
        }
        else if (attackReady[1])
        {
            StartCoroutine(IndividualSkill());
        }
        else if (attackReady[0])
        {
            StartCoroutine(BasicAttack());
        }
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator BasicAttack()
    {
        attackReady[0] = false;

        if (eHS.bossPhase > 0)
        {
            noOFBAs++;
        }

        if (noOFBAs == 3)
        {
            StartCoroutine(AdvancedBasicAttack());  
        }
        else
        {
            //Play attack anim
            target.GetComponent<PlayerControlScript>().TakeDamage(skillDMGScale[0] * attack);
        }

        StartCoroutine(ResetCooldown(0, attackCooldowns[0]));
        yield return null;
    }

    public IEnumerator AdvancedBasicAttack()
    {
        attackReady[0] = false;
        noOFBAs = 0;

        //Play attack anim
        target.GetComponent<PlayerControlScript>().TakeDamage(skillDMGScale[1] * attack);

        StartCoroutine(ResetCooldown(0, attackCooldowns[0]));
        yield return null;
    }

    public IEnumerator IndividualSkill()
    {
        attackReady[1] = false;

        if (eHS.bossPhase == 2)
        {
            //Play attack anim
            //stun the player
        }
        else if (eHS.bossPhase == 1)
        {
            //play summon anim
            GameObject eco = Instantiate(gauntGeneral, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            eco.GetComponent<Study_Gaunt_General_Script>().target = target;
        }
        else
        {
            //play summon anim
            GameObject eco = Instantiate(errorCodeOrb, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            eco.GetComponent<Error_Code_Orb_Script>().target = target;
        }

        StartCoroutine(ResetCooldown(1, attackCooldowns[1]));
        yield return null;
    }

    public IEnumerator Ultimate()
    {
        attackReady[2] = false;

        ultIsActive = true;

        //play charge up anim

        yield return new WaitForSeconds(5);

        //play attack anim

        if (takenHits < 30 && target.GetComponent<PlayerControlScript>().hasIFrames == false)
        {
            target.GetComponent<PlayerControlScript>().PlayerDeath();
        }

        eHS.hasIFrames = false;

        StartCoroutine(ResetCooldown(2, attackCooldowns[2]));
        yield return null;
    }

    public IEnumerator ResetCooldown(int skillNO, float cooldownLength)
    {
        yield return new WaitForSeconds(cooldownLength);

        yield return attackReady[skillNO] = true;
    }

}
