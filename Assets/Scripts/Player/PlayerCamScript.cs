using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamScript : MonoBehaviour
{
    public Transform camPos;

    public Vector3 camOffset;

    // Update is called once per frame
    void Update()
    {
        transform.position = (camPos.position + camOffset);
    }
}
