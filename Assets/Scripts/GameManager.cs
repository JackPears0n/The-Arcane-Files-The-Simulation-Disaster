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

    private ProgressionScript ps;

    [Header("Player")]
    public GameObject playerObject;
    public PlayerControlScript pcs;
    public Stats stats;
    public char chosenPlayer;

    [Header("Level")]
    public int lvlNum;
    public char lvlType;
    public float difficulty;

    [Header("UI")]
    public GameObject ui;
    public GameObject hud;
    public GameObject shop;
    public GameObject victoryUI;
    public GameObject defeatUI;

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
        ps = gameObject.GetComponent<ProgressionScript>();

    }

    // Update is called once per frame
    void Update()
    {
        //Gets the game info
        if (playerObject == null)
        {
            GetGameInfo();
        }

        //Temporary button to progress levels      
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (ps.canProgressToNextLvl)
            {
                ps.NextLevel();
            }
        }

        //Pauses time
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }


       if (Input.GetKeyDown(KeyCode.Escape) && !shop.activeSelf)
       {
           PauseGame();
       }

        if (Input.GetKeyDown(KeyCode.Escape) && shop.activeSelf)
        {
            ToggleShop();
        }

        if (lvlType == 'S' && shop != null)
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

    public void SetDifficulty(float dif)
    {
        difficulty = dif;
    }

    public void GetGameInfo()
    {
        playerObject = GameObject.Find("Player Object");
        pcs = playerObject.GetComponent<PlayerControlScript>();
        pcs.chosenPlayer = chosenPlayer;
        stats = playerObject.GetComponent<PlayerControlScript>().stats;

        ui = GameObject.Find("UI");
        hud = GameObject.Find("HUD");
        shop = GameObject.Find("Shop Container");

        victoryUI = GameObject.Find("Victory UI");
        victoryUI.SetActive(false);

        defeatUI = GameObject.Find("Defeat UI");
        defeatUI.SetActive(false);

        lvlNum = 0;
    }
}