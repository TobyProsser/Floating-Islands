using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonLookController : MonoBehaviour
{
    public float mouseSensitivity = 100;

    public Transform firstPersonPlayer;

    float xRotation = 0f;

    public bool canLook;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canLook = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canLook)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            firstPersonPlayer.Rotate(Vector3.up * mouseX);
        }
    }
}
