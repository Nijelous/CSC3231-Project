using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Easy movement for the floating camera, including moving the camera with the mouse
public class Movement : MonoBehaviour
{
    private float pitch = 0.0f;
    private float yaw = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * 50;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * 50;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Time.deltaTime * 50;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * 50;
        }
        yaw += 10 * Input.GetAxis("Mouse X");
        pitch -= 10 * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

    }
}
