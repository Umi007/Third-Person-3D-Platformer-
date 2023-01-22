using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateScript : MonoBehaviour
{
    private float x;
    private float y;
    public float rotateSensitivity = -2f;
    private Vector3 rotate;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        rotate = new Vector3(0, x  * rotateSensitivity, 0);
        transform.eulerAngles = transform.eulerAngles - rotate;
    }
}