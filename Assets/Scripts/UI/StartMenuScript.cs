using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartMenuScript : MonoBehaviour
{
    public GameManager gm;

    public TMP_Text chosenCharacter;
    public TMP_Text chosenDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GM").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chosenCharacter.gameObject.activeSelf)
        {
            if (gm.chosenPlayer == 'T')
            {
                chosenCharacter.text = "Character: \n Thomas";
            }
            if (gm.chosenPlayer == 'A')
            {
                chosenCharacter.text = "Character: \n Ada";
            }
            if (gm.chosenPlayer == 'K')
            {
                chosenCharacter.text = "Character: \n Kris";
            }
            if (gm.chosenPlayer == 'E')
            {
                chosenCharacter.text = "Character: \n Eliana";
            }
        }
        if (chosenDifficulty.gameObject.activeSelf)
        {
            chosenDifficulty.text = "Difficulty: " + gm.difficulty.ToString();
        }
    }

    public void SetCharacter(string c)
    {
        char[] p = c.ToCharArray();
        gm.chosenPlayer = p[0];
    }

    public void SetDifficulty(float d)
    {
        gm.difficulty = d;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }
}
