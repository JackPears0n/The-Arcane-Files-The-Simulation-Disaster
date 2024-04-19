using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamScript : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            // Locks the cursor in place
            Cursor.lockState = CursorLockMode.None;

            // Makes cursor incisible
            Cursor.visible = true;

            // Pauses any physics
            Time.timeScale = 0;

        }
        else
        {
            // Resumes physics
            Time.timeScale = 1;

            // Locks the cursor in place
            Cursor.lockState = CursorLockMode.Locked;

            // Makes cursor invisable
            Cursor.visible = false;

            transform.rotation = camOrientation.rotation;

            // Getting the mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSens;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySens;

            yRotate += mouseX;
            xRotate -= mouseY;


            // Clamps the axis view between
            xRotate = Mathf.Clamp(xRotate, -70, 70);

            // Roate camera and its orientation
            transform.rotation = Quaternion.Euler(xRotate, yRotate, 0);
            camOrientation.rotation = Quaternion.Euler(0, yRotate, 0);
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
