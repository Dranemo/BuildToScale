using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    private bool jumpInput = false;

    Vector3 movementX;
    Vector3 movementZ;



    // -------------------------------------------------------------------------------- Functions Unity -------------------------------------------------------------------------------- //
    void Update()
    {
        if (Pause.paused)
        {
            Debug.Log("N=Pauszed Moveet");
            horizontalInput = 0f;
            verticalInput = 0f;
            jumpInput = false;

            return;
        }


        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        jumpInput = Input.GetButtonDown("Jump");

        if(IsGrounded())
            jumpInput = Input.GetButtonDown("Jump");
        else
            jumpInput = false;

        // Mouvement
        movementZ = transform.TransformDirection(Vector3.forward) * verticalInput * speed;
        movementX = transform.TransformDirection(Vector3.right) * horizontalInput * speed;

    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(movementX);
        GetComponent<Rigidbody>().AddForce(movementZ);

        if(jumpInput)
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    }

    private bool IsGrounded()
    {
        // Longueur du raycast
        float rayLength = 1.5f;
        // Position de départ du raycast (légèrement au-dessus du joueur)
        Vector3 rayOrigin = transform.position;


        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red);
        // Effectuer le raycast
        return Physics.Raycast(rayOrigin, Vector3.down, rayLength);
    }

}
