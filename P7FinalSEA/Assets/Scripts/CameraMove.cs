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
        if ((GameObject.Find("Player").GetComponent<PlayerControls>().left != 0 || GameObject.Find("Player").GetComponent<PlayerControls>().forward != 0) && !GameObject.Find("Player").GetComponent<PlayerControls>().dashing)
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, 60f + 10f * (GameObject.Find("Player").GetComponent<PlayerControls>().speed/GameObject.Find("Player").GetComponent<PlayerControls>().speedInit), 0.03f);
        }
        else if (GameObject.Find("Player").GetComponent<PlayerControls>().dashing)
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, 80f, 0.03f);
        }
        else
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, 60f, 0.03f);
        }
        Cursor.lockState = CursorLockMode.Locked;
        rotate = Input.GetAxis("Mouse X");
        votate = Input.GetAxis("Mouse Y");
        transform.RotateAround(transform.position, Vector3.up, rotate * 720 * sensitivity * Time.deltaTime);
        transform.Rotate(Vector3.right, votate * -720 * sensitivity* Time.deltaTime);
        transform.position = GameObject.Find("Player").transform.position + Vector3.up*0.5f;
        if (transform.localEulerAngles.x < 280 && transform.localEulerAngles.x > 180)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(280, transform.localEulerAngles.y, transform.localEulerAngles.z));
        }
        else if (transform.localEulerAngles.x > 80 && transform.localEulerAngles.x < 180)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(80, transform.localEulerAngles.y, transform.localEulerAngles.z));
        }

        if (transform.localEulerAngles.z != 0 && transform.localEulerAngles.z != 180)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0));
        }

        Debug.Log(transform.localEulerAngles.x);
    }
}
