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
        }
        //Level 1 logic
        else if (gm.lvlNum == 1)
        {
            gm.lvlType = 'C';

            //Spawns the first wave of enemies
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 4;
                SpawnEnemy(ae, husk);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (activeEnemies[4] == null && spawnedEnemies)
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
                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 1 && activeEnemies[0] == null && activeEnemies[1] == null && spawnedEnemies)
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
            if (waveNum == 0 && !waveBeaten && !spawnedEnemies)
            {
                ae = 4;
                SpawnEnemy(ae, aArachnid);

                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (activeEnemies[4] == null && spawnedEnemies)
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
                SpawnEnemy(ae, aArachnid);

                ae = 1;
                SpawnEnemy(ae, husk);
                spawnedEnemies = true;
            }

            //Checks to see if the first wave are dead
            if (waveNum == 1 && activeEnemies[0] == null && activeEnemies[1] == null && spawnedEnemies)
            {
                waveBeaten = true;
                canProgressToNextLvl = true;
                spawnedEnemies = false;
            }
        }
        //Level 3 logic
        else if (gm.lvlNum == 3)
        {

        }
        //Level 4 logic
        else if (gm.lvlNum == 4)
        {

        }
        //Level 5 logic
        else if (gm.lvlNum == 5)
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
        //Level 6 logic
        else if (gm.lvlNum == 6)
        {

        }
        //Level 7 logic
        else if (gm.lvlNum == 7)
        {

        }
        //Level 8 logic
        else if (gm.lvlNum == 8)
        {

        }
        //Level 9 logic
        else if (gm.lvlNum == 9)
        {

        }
        //Level 10 logic
        else if (gm.lvlNum == 10)
        {

        }
        //Level 11 logic
        else if (gm.lvlNum == 11)
        {

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
        }
        //Level 1 logic
        else if (gm.lvlNum == 1)
        {
            
        }
        //Level 2 logic
        else if (gm.lvlNum == 2)
        {

        }
        //Level 3 logic
        else if (gm.lvlNum == 3)
        {

        }
        //Level 4 logic
        else if (gm.lvlNum == 4)
        {

        }
        //Level 5 logic
        else if (gm.lvlNum == 5)
        {

        }
        //Level 6 logic
        else if (gm.lvlNum == 6)
        {

        }
        //Level 7 logic
        else if (gm.lvlNum == 7)
        {

        }
        //Level 8 logic
        else if (gm.lvlNum == 8)
        {

        }
        //Level 9 logic
        else if (gm.lvlNum == 9)
        {

        }
        //Level 10 logic
        else if (gm.lvlNum == 10)
        {

        }
        //Level 11 logic
        else if (gm.lvlNum == 11)
        {

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
        }
        //Level 1 logic
        else if (gm.lvlNum == 1)
        {

        }
        //Level 2 logic
        else if (gm.lvlNum == 2)
        {

        }
        //Level 3 logic
        else if (gm.lvlNum == 3)
        {

        }
        //Level 4 logic
        else if (gm.lvlNum == 4)
        {

        }
        //Level 5 logic
        else if (gm.lvlNum == 5)
        {

        }
        //Level 6 logic
        else if (gm.lvlNum == 6)
        {

        }
        //Level 7 logic
        else if (gm.lvlNum == 7)
        {

        }
        //Level 8 logic
        else if (gm.lvlNum == 8)
        {

        }
        //Level 9 logic
        else if (gm.lvlNum == 9)
        {

        }
        //Level 10 logic
        else if (gm.lvlNum == 10)
        {

        }
        //Level 11 logic
        else if (gm.lvlNum == 11)
        {

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
        }
        //Level 1 logic
        else if (gm.lvlNum == 1)
        {

        }
        //Level 2 logic
        else if (gm.lvlNum == 2)
        {

        }
        //Level 3 logic
        else if (gm.lvlNum == 3)
        {

        }
        //Level 4 logic
        else if (gm.lvlNum == 4)
        {

        }
        //Level 5 logic
        else if (gm.lvlNum == 5)
        {

        }
        //Level 6 logic
        else if (gm.lvlNum == 6)
        {

        }
        //Level 7 logic
        else if (gm.lvlNum == 7)
        {

        }
        //Level 8 logic
        else if (gm.lvlNum == 8)
        {

        }
        //Level 9 logic
        else if (gm.lvlNum == 9)
        {

        }
        //Level 10 logic
        else if (gm.lvlNum == 10)
        {

        }
        //Level 11 logic
        else if (gm.lvlNum == 11)
        {

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

    public void NextLevel()
    {
        if (canProgressToNextLvl)
        {
            waveNum = 0;
            waveBeaten = false;

            canProgressToNextLvl = false;
            gm.playerObject.transform.position = new Vector3(0, 0.28f, -6.45f);

            gm.lvlNum++;
        }
    }
}
