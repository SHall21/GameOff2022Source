using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingMarm : MonoBehaviour
{
    public int rotatecheck = 1;
    public float repeatTimeLeft = 0.0f;
    public float repeatTimeRight = 3.0f;
    public float repeatRate = 6.0f;
    public int rotateSpeed = 40;

    void Start()
    {

        InvokeRepeating("MarmLeft", repeatTimeLeft, repeatRate);
        InvokeRepeating("MarmRight", repeatTimeRight, repeatRate);

    }

    void FixedUpdate()
    {
        if (rotatecheck == 1)
        {
            transform.position = transform.position + new Vector3(1 * Time.deltaTime, 0, 0);
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * -rotateSpeed));
        }

        if (rotatecheck == 2)
        {
            transform.position = transform.position + new Vector3(-1 * Time.deltaTime, 0, 0);
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotateSpeed));
        }
    }
    void MarmLeft()
    {

        rotatecheck = 1;
    }

    void MarmRight()
    {
        rotatecheck = 2;

    }
}
