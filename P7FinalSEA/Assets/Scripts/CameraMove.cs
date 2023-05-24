using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float rotate;
    float votate;
    public float sensitivity = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rotate = Input.GetAxis("Mouse X");
        votate = Input.GetAxis("Mouse Y");
        transform.RotateAround(transform.position, Vector3.up, rotate * 720 * sensitivity * Time.deltaTime);
        transform.Rotate(Vector3.right, votate * -720 * sensitivity* Time.deltaTime);
        transform.position = GameObject.Find("Player").transform.position + Vector3.up*0.5f;
        if (transform.rotation.x >= 90)
        {
            transform.rotation = Quaternion.Euler(new Vector3(90, transform.rotation.y, transform.rotation.z));
        }
        if (transform.rotation.x <= -90)
        {
            transform.rotation = Quaternion.Euler(new Vector3(-90, transform.rotation.y, transform.rotation.z));
        }
    }
}
