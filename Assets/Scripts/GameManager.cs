using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Game")]
    //Pauses will not affect the GameManager
    //Master pause
    public bool paused;
    //Pauses all Update() methods
    public bool logicPaused;
    //Pauses all FixedUpdate() methods
    public bool physicsPaused;

    [Header("Player")]
    public GameObject playerObject;
    public PlayerControlScript PCS;
    public Stats stats;

    [Header("Level")]
    public int lvlNum;
    public char lvlType;

    [Header("UI")]
    public GameObject ui;
    public GameObject hud;
    public GameObject shop;

    private void Awake()
    {
        //Makes sure there is only ever one GM
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Pauses time
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }


       if (Input.GetKeyDown(KeyCode.Escape))
       {
           PauseGame();
       }

       if (lvlType == 'S')
       {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ToggleShop();
            }

            if (!shop.activeSelf)
            {
                paused = false;
            }
        }

    }

    public void PauseGame()
    {
        if (!paused)
        {
            paused = true;
        }
        else if (paused)
        {
            paused = false;
        }
        else
        {
            return;
        }
    }

    public void ToggleShop()
    {
        if (shop.activeSelf)
        {
            hud.SetActive(true);
            shop.SetActive(false);
            paused = false;
        }
        else
        {
            hud.SetActive(false);
            shop.SetActive(true);
            paused = true;
        }
    }
}
