using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuScript : MonoBehaviour
{
    [Header ("Player")]
    public GameObject player;
    public PlayerControlScript pcs;

    [Header("UI")]
    public TMP_Text maxHPTXT;
    public TMP_Text attackTXT;
    public TMP_Text defenceTXT;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player Object");
        pcs = player.GetComponent<PlayerControlScript>();

        maxHPTXT.text = "Max Health: " + pcs.maxHP.ToString();
        attackTXT.text = "Attack: " + pcs.attack.ToString();
        defenceTXT.text = "Defence: " + pcs.defence.ToString();
    }
}
