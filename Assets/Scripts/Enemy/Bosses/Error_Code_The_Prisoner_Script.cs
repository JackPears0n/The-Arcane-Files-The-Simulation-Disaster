using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Error_Code_The_Prisoner_Script : MonoBehaviour
{
    public GameObject[] models;
    public Animator[] anims;
    public TMP_Text p3txt;

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
    public Slider ultNumSlider;
    public ParticleSystem ult_ps;

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
        if (ultNumSlider.gameObject.activeSelf)
        {
            ultNumSlider.value = takenHits;
        }

        if (eHS.bossPhase == 0)
        {
            //Activates the current model
            models[0].SetActive(true);

            //Updates the EHS
            eHS.model = models[0];
            eHS.maxHP = 10000;
            attack = 150;
            StartCoroutine(Phase1Brain());
        }

        if (eHS.bossPhase == 1)
        {
            //Activates the current model and deactivates the old
            models[0].SetActive(false);
            models[1].SetActive(true);

            //Updates the EHS
            eHS.model = models[1];
            eHS.maxHP = 8000;
            attack = 175;
            StartCoroutine(Phase2Brain());
        }

        if (eHS.bossPhase == 2)
        {
            //Activates the current model and deactivates the old
            models[1].SetActive(false);
            models[2].SetActive(true);
            p3txt.gameObject.SetActive(true);

            //Updates the EHS
            eHS.model = models[2];
            eHS.anim = anims[2];

            eHS.maxHP = 6000;
            attack = 200;
            StartCoroutine(Phase3Brain());
        }

        if (eHS.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && eHS.anim.GetCurrentAnimatorStateInfo(0).IsName("BA"))
        {
            eHS.anim.SetBool("BA", false);
        }
        if (eHS.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && eHS.anim.GetCurrentAnimatorStateInfo(0).IsName("IS"))
        {
            eHS.anim.SetBool("IS", false);
        }
        if (eHS.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85 && eHS.anim.GetCurrentAnimatorStateInfo(0).IsName("ULT"))
        {
            eHS.anim.SetBool("ULT", false);
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

        //Animate
        eHS.anim.SetBool("BA", true);
        eHS.anim.SetBool("IS", false);
        eHS.anim.SetBool("ULT", false);

        if (noOFBAs == 3)
        {
            noOFBAs = 0;
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
            //Animate
            eHS.anim.SetBool("BA", false);
            eHS.anim.SetBool("IS", true);
            eHS.anim.SetBool("ULT", false);

            //stun the player
            eHS.gm.logicPaused = true;
            eHS.gm.physicsPaused = true;

            StartCoroutine(RemovePlayerStun());
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

        ultNumSlider.gameObject.SetActive(true);

        ultIsActive = true;

        //play charge up anim
        ult_ps.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);

        //stop charge up anim
        ult_ps.gameObject.SetActive(false);

        ultNumSlider.gameObject.SetActive(false);

        if (takenHits < 30 && target.GetComponent<PlayerControlScript>().hasIFrames == false)
        {
            //Animate
            eHS.anim.SetBool("BA", false);
            eHS.anim.SetBool("IS", false);
            eHS.anim.SetBool("ULT", true);

            target.GetComponent<PlayerControlScript>().PlayerDeath();
        }

        ultIsActive = false;
        eHS.hasIFrames = false;

        StartCoroutine(ResetCooldown(2, attackCooldowns[2]));
        yield return null;
    }

    public IEnumerator ResetCooldown(int skillNO, float cooldownLength)
    {
        yield return new WaitForSeconds(cooldownLength);

        yield return attackReady[skillNO] = true;
    }

    public IEnumerator RemovePlayerStun()
    {
        yield return new WaitForSeconds(1.5f);

        //Remove player stun
        eHS.gm.logicPaused = true;
        eHS.gm.physicsPaused = true;

        yield return null;
    }

}
