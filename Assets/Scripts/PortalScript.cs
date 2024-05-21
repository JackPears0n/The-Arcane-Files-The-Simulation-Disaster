using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public GameObject portalModel;

    public GameObject player;
    public PlayerControlScript pcs;
    public LayerMask playerLayers;

    public GameObject gameManager;
    public GameManager gm;
    public ProgressionScript ps;

    public bool isATrigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameManager = GameObject.Find("GM");
        gm = gameManager.GetComponent<GameManager>();
        ps = gameManager.GetComponent<ProgressionScript>();

        if (!isATrigger)
        {
            if (ps.canProgressToNextLvl)
            {
                portalModel.SetActive(true);
                bool p = Physics.CheckSphere(transform.position, 3, playerLayers);
                if (p)
                {
                    if (Input.GetKeyDown(KeyCode.L) && !pcs.playerIsDead)
                    {
                        ps.NextLevel();
                    }
                }
            }
            else
            {
                portalModel.SetActive(false);
                return;
            }
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isATrigger)
        {
            if (other.gameObject.tag == "Player")
            {
                ps.NextLevel();
            }
        }
    }
}
