using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Error_Code_OrbProjScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public GameObject owner;
    private Error_Code_Orb_Script ownerCS;

    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            agent.destination = target.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target.gameObject.GetComponent<PlayerControlScript>().TakeDamage(ownerCS.attack * ownerCS.skillDMGScale[0]);

            Destroy(gameObject);
        }
    }

    public void GetInfo(GameObject _owner, GameObject player)
    {
        owner = _owner;
        target = player;

        ownerCS = owner.GetComponent<Error_Code_Orb_Script>();
    }
}
