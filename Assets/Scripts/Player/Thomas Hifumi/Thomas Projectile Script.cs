using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThomasProjectileScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public GameObject thomas;
    private ThomasCombatScript thomasCS;

    public GameObject target;
    private EnemyHealthScript targetHS;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        if (other.tag == "Enemy")
        {
            targetHS.TakeDamage(thomasCS.bAttack2DMGScale * thomasCS.attack);
            Destroy(gameObject);
        }
    }

    public void GetInfo(GameObject player, GameObject enemy)
    {
        thomas = player;
        target = enemy;

        thomasCS = thomas.GetComponent<ThomasCombatScript>();
        targetHS = target.GetComponent<EnemyHealthScript>();
    }
}
