using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControlScript : MonoBehaviour
{
    public GameObject player;

    private NavMeshAgent navMeshAgent;

    Vector2 movementInput;
    

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
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
        navMeshAgent.destination = destination;
    }

    public void ROTATE(Vector2 input)
    {
        navMeshAgent.destination = transform.position;
        transform.Rotate(0, input.x * navMeshAgent.angularSpeed * Time.deltaTime, 0);
    }
}
