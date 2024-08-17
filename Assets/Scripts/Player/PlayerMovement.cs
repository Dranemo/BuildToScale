using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private float speed = 5f;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    Vector3 movementX;
    Vector3 movementZ;

    bool movementLocked = false;



    // -------------------------------------------------------------------------------- Functions Unity -------------------------------------------------------------------------------- //
    void Update()
    {
        if (!movementLocked)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            // Mouvement
            movementZ = transform.TransformDirection(Vector3.forward) * verticalInput * speed;
            movementX = transform.TransformDirection(Vector3.right) * horizontalInput * speed;
        }

    }

    private void FixedUpdate()
    {
        if (!movementLocked)
        {
            GetComponent<Rigidbody>().AddForce(movementX);
            GetComponent<Rigidbody>().AddForce(movementZ);
        }
    }


    public void SetLockMouvement(bool booleen)
    {
        movementLocked = booleen;
    }
}
