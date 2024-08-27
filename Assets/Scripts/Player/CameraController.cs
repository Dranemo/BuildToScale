using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] public float mouseSensitivity = 500f;

    private float xRotationCamera = 0f;
    float rotationX;
    float rotationY;

    [SerializeField] private Transform playerBody;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Pause.paused)
        {
            rotationX = 0f;
            rotationY = 0f;
            return;
        }

        rotationX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotationY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
    }

    private void LateUpdate()
    {
        xRotationCamera -= rotationY;
        xRotationCamera = Mathf.Clamp(xRotationCamera, -90f, 90f);


        transform.localRotation = Quaternion.Euler(xRotationCamera, 0f, 0f);
        playerBody.Rotate(Vector3.up * rotationX);
    }
}
