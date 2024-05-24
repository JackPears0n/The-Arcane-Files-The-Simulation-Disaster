using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ProgressionScript : MonoBehaviour
{
    public GameObject[] enemySpawnPoints;
    public GameManager gm;

    public bool canProgressToNextLvl;

    //Int used for determaning which enemy will spawn and where it will go
    public int ae;

    [Header("Enemy Game Objects")]
    public GameObject husk;
    public GameObject aArachnid;
    public GameObject orb;
    public GameObject aSQueen;
    public GameObject gGeneral;
    public GameObject prisoner;

    public GameObject[] activeEnemies;

    public bool spawnedEnemies;
    public bool waveBeaten;

    [Header("UI")]
    public TMP_Text levelText;

    public int waveNum;
    public TMP_Text waveTXT;

    // Start is called before the first frame update
    void Start()
    {
        gm = gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GetSpawnPoints();

            if (levelText == null)
            {
                levelText = GameObject.Find("Level Num Text").GetComponent<TMP_Text>();
            }
            levelText.text = "Current Level:" + gm.lvlNum.ToString();

            if (waveTXT == null)
            {
                waveTXT = GameObject.Find("Level Wave Text").GetComponent<TMP_Text>();
            }

            if (gm.lvlNum != 0 && gm.lvlNum != 5 && gm.lvlNum != 10)
            {
                waveTXT.text = "Current Wave:" + (waveNum + 1).ToString();
            }
            else
            {
                waveTXT.text = "Shop Level";
            }          
        }

        if (!gm.paused)
        {
            if (!gm.logicPaused)
            {
                if (SceneManager.GetActiveScene().name == "Game")
                {
                    ModeSelector(gm.difficulty);
                }
            }
        }
    }

    public void ModeSelector(float dif)
    {
        if (dif == 0.5f)
        {
            StartCoroutine(EasyMode());
        }

        if (dif == 1)
        {
            StartCoroutine(NormalMode());
        }

        if (dif == 1.5f)
        {
            StartCoroutine(HardMode());
        }

        if (dif == 2)
        {
            StartCoroutine(UnknownMode());
        }
    }

    public IEnumerator EasyMode()
    {
        //Opening Level logic
        if (gm.lvlNum == 0)
        {
            //Sets level type to shop
            gm.lvlType = 'S';

            canProgressToNextLvl = true;
        }
        //Level 1 logic
        else if (gm.lvlNum == 1)
        {
            gm.lvlType = 'C';

            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 4;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (activeEnemies[4] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 2 logic
        else if (gm.lvlNum == 2)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 0;
                SpawnEnemy(ae, husk);

                ae = 1;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (activeEnemies[0] == null && activeEnemies[1] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 3 logic
        else if (gm.lvlNum == 3)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 4;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (activeEnemies[4] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 4 logic
        else if (gm.lvlNum == 4)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 5;
                SpawnEnemy(ae, orb);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (activeEnemies[5] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 5 logic
        else if (gm.lvlNum == 5)
        {
            //Sets level type to shop
            gm.lvlType = 'S';

            canProgressToNextLvl = true;
        }
        //Level 6 logic
        else if (gm.lvlNum == 6)
        {
            gm.lvlType = 'C';

            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                ae = 4;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 &&activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[4] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the second wave of enemies
            if (waveNum == 1 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 4;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 1 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[4] == null && spawnedEnemies)
            {
                waveBeaten = false;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 7 logic
        else if (gm.lvlNum == 7)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 3;
                SpawnEnemy(ae, husk);

                ae = 5;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the second wave of enemies
            if (waveNum == 1 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                ae = 3;
                SpawnEnemy(ae, aArachnid);

                ae = 5;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 1 && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveBeaten = false;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 8 logic
        else if (gm.lvlNum == 8)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 5;
                SpawnEnemy(ae, aSQueen);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (activeEnemies[5] == null && spawnedEnemies)
            {
                gm.PauseGame();
                gm.victoryUI.SetActive(true);
            }

        }

        yield return null;
    }

    public IEnumerator NormalMode()
    {
        //Opening Level logic
        if (gm.lvlNum == 0)
        {
            //Sets level type to shop
            gm.lvlType = 'S';

            canProgressToNextLvl = true;
        }
        //Level 1 logic
        else if (gm.lvlNum == 1)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 2 logic
        else if (gm.lvlNum == 2)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 4;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && waveNum == 1 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[4] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 3 logic
        else if (gm.lvlNum == 3)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 4 logic
        else if (gm.lvlNum == 4)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 5;
                SpawnEnemy(ae, orb);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[5] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 5 logic
        else if (gm.lvlNum == 5)
        {
            //Sets level type to shop
            gm.lvlType = 'S';

            canProgressToNextLvl = true;
        }
        //Level 6 logic
        else if (gm.lvlNum == 6)
        {
            gm.lvlType = 'C';

            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                ae = 5;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the second wave of enemies
            if (waveNum == 1 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 5;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 1 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 7 logic
        else if (gm.lvlNum == 7)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                ae = 3;
                SpawnEnemy(ae, husk);

                ae = 4;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the second wave of enemies
            if (waveNum == 1 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 0;
                SpawnEnemy(ae, husk);

                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 1 && activeEnemies[0] == null && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;

            }

            if (waveNum == 2 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 0;
                SpawnEnemy(ae, aArachnid);

                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 2 && activeEnemies[0] == null && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 8 logic
        else if (gm.lvlNum == 8)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 5;
                SpawnEnemy(ae, aSQueen);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[5] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 9 logic
        else if (gm.lvlNum == 9)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 0;
                SpawnEnemy(ae, orb);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 3;
                SpawnEnemy(ae, husk);

                ae = 4;
                SpawnEnemy(ae, husk);

                ae = 5;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[0] == null && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 10 logic
        else if (gm.lvlNum == 10)
        {
            //Sets level type to shop
            gm.lvlType = 'S';

            canProgressToNextLvl = true;
        }
        //Level 11 logic
        else if (gm.lvlNum == 11)
        {
            gm.lvlType = 'C';

            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 0;
                SpawnEnemy(ae, gGeneral);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[0] == null && spawnedEnemies)
            {
                gm.PauseGame();
                gm.victoryUI.SetActive(true);

            }

        }

        yield return null;
    }

    public IEnumerator HardMode()
    {
        //Opening Level logic
        if (gm.lvlNum == 0)
        {
            //Sets level type to shop
            gm.lvlType = 'S';

            canProgressToNextLvl = true;
        }
        //Level 1 logic
        else if (gm.lvlNum == 1)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {

                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the first wave of enemies
            if (waveNum == 1 && !spawnedEnemies)
            {
                ae = 0;
                SpawnEnemy(ae, husk);

                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 1 && activeEnemies[0] == null && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 2 logic
        else if (gm.lvlNum == 2)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 5;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the first wave of enemies
            if (waveNum == 1 && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                ae = 5;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 1 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 3 logic
        else if (gm.lvlNum == 3)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                ae = 3;
                SpawnEnemy(ae, aArachnid);

                ae = 4;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 4 logic
        else if (gm.lvlNum == 4)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, orb);

                ae = 2;
                SpawnEnemy(ae, orb);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 5 logic
        else if (gm.lvlNum == 5)
        {
            //Sets level type to shop
            gm.lvlType = 'S';

            canProgressToNextLvl = true;
        }
        //Level 6 logic
        else if (gm.lvlNum == 6)
        {
            gm.lvlType = 'C';

            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 3;
                SpawnEnemy(ae, aArachnid);

                ae = 4;
                SpawnEnemy(ae, aArachnid);

                ae = 5;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the second wave of enemies
            if (waveNum == 1 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 3;
                SpawnEnemy(ae, aArachnid);

                ae = 4;
                SpawnEnemy(ae, aArachnid);

                ae = 5;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;

            }

            //Checks to see if the first wave are dead
            if (waveNum == 1 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 7 logic
        else if (gm.lvlNum == 7)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                ae = 3;
                SpawnEnemy(ae, husk);

                ae = 4;
                SpawnEnemy(ae, husk);

                ae = 5;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the second wave of enemies
            if (waveNum == 1 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 0;
                SpawnEnemy(ae, husk);

                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the second wave are dead
            if (waveNum == 1 && activeEnemies[0] == null && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the third wave of enemies
            if (waveNum == 2 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 0;
                SpawnEnemy(ae, aArachnid);

                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the third wave are dead
            if (waveNum == 2 && activeEnemies[0] == null && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 8 logic
        else if (gm.lvlNum == 8)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 5;
                SpawnEnemy(ae, aSQueen);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[5] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 9 logic
        else if (gm.lvlNum == 9)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 3;
                SpawnEnemy(ae, orb);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the second wave of enemies
            if (waveNum == 1 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 3;
                SpawnEnemy(ae, aArachnid);

                ae = 4;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the second wave are dead
            if (waveNum == 1 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the third wave of enemies
            if (waveNum == 2 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 1;
                SpawnEnemy(ae, orb);

                ae = 2;
                SpawnEnemy(ae, orb);

                spawnedEnemies = true;
            }

            //Checks to see if the third wave are dead
            if (waveNum == 2 && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }

        }
        //Level 10 logic
        else if (gm.lvlNum == 10)
        {
            //Sets level type to shop
            gm.lvlType = 'S';

            canProgressToNextLvl = true;
        }
        //Level 11 logic
        else if (gm.lvlNum == 11)
        {
            gm.lvlType = 'C';

            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 0;
                SpawnEnemy(ae, gGeneral);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[0] == null && spawnedEnemies)
            {
                gm.PauseGame();
                gm.victoryUI.SetActive(true);

            }

        }

        yield return null;
    }

    public IEnumerator UnknownMode()
    {
        //Opening Level logic
        if (gm.lvlNum == 0)
        {
            //Sets level type to shop
            gm.lvlType = 'S';

            canProgressToNextLvl = true;
        }
        //Level 1 logic
        else if (gm.lvlNum == 1)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {

                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the second wave of enemies
            if (waveNum == 1 && !spawnedEnemies)
            {
                ae = 0;
                SpawnEnemy(ae, orb);

                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 3;
                SpawnEnemy(ae, aArachnid);

                ae = 4;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the second wave are dead
            if (waveNum == 1 && activeEnemies[0] == null &&  activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 2 logic
        else if (gm.lvlNum == 2)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 0;
                SpawnEnemy(ae, orb);

                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 5;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[0] == null && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the first wave of enemies
            if (waveNum == 1 && !spawnedEnemies)
            {
                ae = 0;
                SpawnEnemy(ae, orb);

                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                ae = 5;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 1 && activeEnemies[0] == null && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 3 logic
        else if (gm.lvlNum == 3)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                ae = 3;
                SpawnEnemy(ae, aArachnid);

                ae = 4;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 4 logic
        else if (gm.lvlNum == 4)
        {//Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, orb);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 5 logic
        else if (gm.lvlNum == 5)
        {
            //Sets level type to shop
            gm.lvlType = 'S';

            canProgressToNextLvl = true;
        }
        //Level 6 logic
        else if (gm.lvlNum == 6)
        {
            gm.lvlType = 'C';

            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 3;
                SpawnEnemy(ae, aArachnid);

                ae = 4;
                SpawnEnemy(ae, aArachnid);

                ae = 5;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the second wave of enemies
            if (waveNum == 1 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                ae = 3;
                SpawnEnemy(ae, husk);

                ae = 4;
                SpawnEnemy(ae, husk);

                ae = 5;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;

            }

            //Checks to see if the first wave are dead
            if (waveNum == 1 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 7 logic
        else if (gm.lvlNum == 7)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                ae = 3;
                SpawnEnemy(ae, husk);

                ae = 4;
                SpawnEnemy(ae, husk);

                ae = 5;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && activeEnemies[5] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the second wave of enemies
            if (waveNum == 1 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 0;
                SpawnEnemy(ae, husk);

                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the second wave are dead
            if (waveNum == 1 && activeEnemies[0] == null && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the third wave of enemies
            if (waveNum == 2 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 0;
                SpawnEnemy(ae, aArachnid);

                ae = 1;
                SpawnEnemy(ae, aArachnid);

                ae = 2;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the third wave are dead
            if (waveNum == 2 && activeEnemies[0] == null && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 8 logic
        else if (gm.lvlNum == 8)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 0;
                SpawnEnemy(ae, gGeneral);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[0] == null && spawnedEnemies)
            {
                waveNum++;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 9 logic
        else if (gm.lvlNum == 9)
        {
            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 3;
                SpawnEnemy(ae, orb);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the second wave of enemies
            if (waveNum == 1 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 1;
                SpawnEnemy(ae, husk);

                ae = 2;
                SpawnEnemy(ae, husk);

                ae = 3;
                SpawnEnemy(ae, aArachnid);

                ae = 4;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the second wave are dead
            if (waveNum == 1 && activeEnemies[1] == null && activeEnemies[2] == null && activeEnemies[3] == null && activeEnemies[4] == null && spawnedEnemies)
            {
                waveNum++;
                spawnedEnemies = false;
                waveBeaten = true;
            }

            //Spawns the third wave of enemies
            if (waveNum == 2 && waveBeaten && !spawnedEnemies)
            {
                waveBeaten = false;

                ae = 1;
                SpawnEnemy(ae, orb);

                ae = 2;
                SpawnEnemy(ae, orb);

                spawnedEnemies = true;
            }

            //Checks to see if the third wave are dead
            if (waveNum == 2 && activeEnemies[1] == null && activeEnemies[2] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }

        }
        //Level 10 logic
        else if (gm.lvlNum == 10)
        {
            //Sets level type to shop
            gm.lvlType = 'S';

            canProgressToNextLvl = true;
        }
        //Level 11 logic
        else if (gm.lvlNum == 11)
        {
            gm.lvlType = 'C';

            //Spawns the first wave of enemies
            if (waveNum == 0 && !spawnedEnemies)
            {
                ae = 0;
                SpawnEnemy(ae, prisoner);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 0 && activeEnemies[0] == null && spawnedEnemies)
            {
                gm.PauseGame();
                gm.victoryUI.SetActive(true);

            }

        }

        yield return null;
    }

    public void SpawnEnemy(int ae, GameObject et)
    {
        activeEnemies[ae] = Instantiate(et, enemySpawnPoints[ae].gameObject.transform.position, enemySpawnPoints[ae].gameObject.transform.rotation);
        if (gm.lvlNum <= 5)
        {
            activeEnemies[ae].GetComponent<EnemyHealthScript>().difficulty = gm.difficulty;
        }
        else if (gm.lvlNum <= 10)
        {
            activeEnemies[ae].GetComponent<EnemyHealthScript>().difficulty = gm.difficulty + 0.5f;
        }
        else if (gm.lvlNum == 11)
        {
            activeEnemies[ae].GetComponent<EnemyHealthScript>().difficulty = gm.difficulty + 1f;
        }
    }

    public IEnumerator NextLevel()
    {
        if (canProgressToNextLvl)
        {
            //gm.screenBlock.SetActive(true);

            waveNum = 0;
            waveBeaten = false;

            canProgressToNextLvl = false;

            gm.lvlNum++;
            yield return new WaitForSeconds(1);
            
            //gm.screenBlock.SetActive(false);
        }
    }

    public void GetSpawnPoints()
    {
        enemySpawnPoints[0] = GameObject.Find("Boss Spawn Point");
        enemySpawnPoints[1] = GameObject.Find("Enemy Spawn Point (1)");
        enemySpawnPoints[2] = GameObject.Find("Enemy Spawn Point (2)");
        enemySpawnPoints[3] = GameObject.Find("Enemy Spawn Point (3)");
        enemySpawnPoints[4] = GameObject.Find("Enemy Spawn Point (4)");
        enemySpawnPoints[5] = GameObject.Find("Enemy Spawn Point (5)");
    }
}
