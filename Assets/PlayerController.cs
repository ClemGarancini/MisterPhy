
using UnityEngine;

using System.Collections.Generic;
using Unity.VisualScripting;
public class PlayerController : MonoBehaviour
{

    // Object characteristic
    public float radius = 1.0f;
    public float mass = 1f;
    public float stiffness = 0.2f;


    // Object kinematic features
    public Vector3 speed; // m/s
    public Vector3 acceleration;

    // Forces

    private Vector3 inputForce;

    private MecanicForces forcesComponent;
    private List<Vector3> externalForces;
    Vector3 totalForce;

    // Input features
    public float moveSpeed = 5f;
    public float jumpForce = 20f;

    public float maxSpeed = 10.0f;



    private float moveInput;

    private bool isGrounded;

    private void Start()
    {
        forcesComponent = GetComponent<MecanicForces>();

        transform.localScale = new Vector3(2 * radius, 2 * radius, 0.0f);

        totalForce = Vector3.zero;

    }

    private void Update()
    {

        totalForce = Vector3.zero;

        moveInput = Input.GetAxisRaw("Horizontal");
        MoveInput();

        externalForces = forcesComponent.ComputeForces(mass, radius, speed, isGrounded, moveInput);

        foreach (Vector3 force in externalForces)
        {
            totalForce += force; // Ajoutez chaque vecteur à la somme totale
        }



        CheckGroundCollision();

        Jump();
        ComputeIntegration();


    }

    private void ComputeIntegration()
    {
        acceleration = totalForce / mass;
        speed += acceleration * Time.deltaTime;
        transform.position += speed * Time.deltaTime;
    }

    private void AccelerationPressed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputForce = 2 * moveInput * forcesComponent.nominalForce;
        }
        SpeedLimit();
    }

    private void CheckGroundCollision()
    {
        if (transform.position.y - radius < 0)
        {
            transform.position += new Vector3(0.0f, radius - transform.position.y - 0.001f, 0.0f);
        }

        if (!isGrounded && transform.position.y <= radius)
        {
            speed.y = -stiffness * speed.y;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            speed = new Vector3(speed.x, jumpForce, 0.0f);
        }
    }

    private void SpeedLimit()
    {
        if (Mathf.Abs(speed.x) > maxSpeed)
        {
            speed.x = maxSpeed * speed.x / Mathf.Abs(speed.x);
        }
    }

    private void MoveInput()
    {
        if (moveInput != 0)
        {
            inputForce = isGrounded ? moveInput * forcesComponent.nominalForce : Vector3.zero;
            AccelerationPressed();
            if (speed.x == 0) speed = new Vector3(moveInput * moveSpeed, speed.y, 0.0f);
            totalForce += inputForce;
        }
        else
        {
            speed = new Vector3(0.0f, speed.y, 0.0f);
        }

    }
}