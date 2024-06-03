using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamScript : MonoBehaviour
{

    //Game Manager
    public GameObject gm;
    public GameManager gms;

    // Mouse sensativity
    public float xSens;
    public float ySens;

    public Transform camOrientation;

    public GameObject characterBody;

    // Camera rotation
    float xRotate;
    float yRotate;

    // Start is called before the first frame update
    void Start()
    {

        // Locks the cursor in place
        Cursor.lockState = CursorLockMode.Locked;

        // Makes cursor incisible
        Cursor.visible = false;

    }

    void Update()
    {
        gm = GameObject.Find("GM");
        gms = gm.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (Input.GetKey(KeyCode.LeftAlt) || gms.paused)
        {
            // Locks the cursor in place
            Cursor.lockState = CursorLockMode.None;

            // Makes cursor incisible
            Cursor.visible = true;

        }
        
        else
        {
            // Locks the cursor in place
            Cursor.lockState = CursorLockMode.Locked;

            // Makes cursor invisable
            Cursor.visible = false;
            /*
            transform.rotation = camOrientation.rotation;

            // Getting the mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSens;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySens;

            yRotate += mouseX;
            xRotate -= mouseY;


            // Clamps the axis view between
            xRotate = Mathf.Clamp(xRotate, -70, 70);
            //yRotate = Mathf.Clamp(yRotate, -70, 70);

            // Roate camera and its orientation
            transform.rotation = Quaternion.Euler(xRotate, yRotate, 0);
            camOrientation.rotation = Quaternion.Euler(xRotate, yRotate, 0);
            */
        }

    }

    private void OnDestroy()
    {
        // Unlocks the cursor in place
        Cursor.lockState = CursorLockMode.None;

        // Makes cursor visable
        Cursor.visible = true;
    }
}
