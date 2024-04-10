using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControlScript : MonoBehaviour
{
    //thomas, ada, eliana, kris
    public GameObject[] players = { };
    public GameObject player;
    public string chosenPlayer; 

    private NavMeshAgent agent;

    Vector2 movementInput;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (chosenPlayer == "Thomas")
        {
            Quaternion r = new Quaternion(0, 0, 0, 0);           
            player = Instantiate(players[0], transform.position, r, gameObject.transform);
        }

        if (chosenPlayer == "Ada")
        {
            Quaternion r = new Quaternion(0, 0, 0, 0);
            player = Instantiate(players[1], transform.position, r, gameObject.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(movementInput.y) > 0.01f)
        {
            Move(movementInput);
        }
        else
        {
            ROTATE(movementInput);
        }
    }

    public void Move(Vector2 input)
    {
        Vector3 destination = transform.position + transform.right * input.x + transform.forward * input.y;
        agent.destination = destination;
    }

    public void ROTATE(Vector2 input)
    {
        agent.destination = transform.position;
        transform.Rotate(0, input.x * agent.angularSpeed * Time.deltaTime, 0);
    }
}
