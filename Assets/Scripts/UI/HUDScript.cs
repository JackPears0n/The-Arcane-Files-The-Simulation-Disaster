using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    public GameObject[] skillIcons;

    public GameObject player;
    public PlayerControlScript pcs;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player Object");
        pcs = player.GetComponent<PlayerControlScript>();

        if (!pcs.skillActive[0])
        {
            skillIcons[0].SetActive(false);
        }
        else
        {
            skillIcons[0].SetActive(true);
        }

        if (!pcs.skillActive[1])
        {
            skillIcons[1].SetActive(false);
        }
        else
        {
            skillIcons[1].SetActive(true);
        }

        if (!pcs.skillActive[2])
        {
            skillIcons[2].SetActive(false);
        }
        else
        {
            skillIcons[2].SetActive(true);
        }

        if (!pcs.skillActive[3])
        {
            skillIcons[3].SetActive(false);
        }
        else
        {
            skillIcons[3].SetActive(true);
        }
    }
}
