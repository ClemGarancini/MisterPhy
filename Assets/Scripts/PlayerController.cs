
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
    public Vector3 velocity; // m/s
    public Vector3 acceleration;

    // Forces

    private Vector3 inputForce;

    private MecanicForces forcesComponent;
    public List<Vector3> externalForces { get; private set; }
    Vector3 totalForce;

    // Input features
    public float movevelocity = 5f;
    public float jumpForce = 20f;

    public float maxvelocity = 10.0f;



    private float moveInput;

    private bool isGrounded;

    private Energy energy;
    public float totalEnergy;
    public float kineticEnergy;
    public float gravitationalPotentialEnergy;


    private void Start()
    {
        forcesComponent = GetComponent<MecanicForces>();

        transform.localScale = new Vector3(2 * radius, 2 * radius, 0.0f);

        totalForce = Vector3.zero;

        energy = new();
        energy.Initialize();
    }

    private void Update()
    {

        totalForce = Vector3.zero;

        moveInput = Input.GetAxisRaw("Horizontal");
        MoveInput();

        externalForces = forcesComponent.ComputeForces(mass, radius, velocity, isGrounded, moveInput);

        foreach (Vector3 force in externalForces)
        {
            totalForce += force; // Ajoutez chaque vecteur à la somme totale
        }


        CheckGroundCollision();
        Jump();
        ComputeIntegration();

        totalEnergy = energy.GetTotalEnergy(mass, -forcesComponent.gravity.y, transform.position.y, velocity);
        kineticEnergy = energy.GetKineticEnergy(mass, velocity);
        gravitationalPotentialEnergy = energy.GetGravitationalPotentialEnergy(mass, -forcesComponent.gravity.y, transform.position.y);



    }

    private void ComputeIntegration()
    {
        acceleration = totalForce / mass;
        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }

    private void AccelerationPressed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputForce = 2 * moveInput * forcesComponent.nominalForce;
        }
        velocityLimit();
    }

    private void CheckGroundCollision()
    {
        if (transform.position.y - radius < 0)
        {
            transform.position += new Vector3(0.0f, radius - transform.position.y - 0.001f, 0.0f);
        }

        if (!isGrounded && transform.position.y <= radius)
        {
            velocity.y = -stiffness * velocity.y;
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
            velocity = new Vector3(velocity.x, jumpForce, 0.0f);
        }
    }

    private void velocityLimit()
    {
        if (Mathf.Abs(velocity.x) > maxvelocity)
        {
            velocity.x = maxvelocity * velocity.x / Mathf.Abs(velocity.x);
        }
    }

    private void MoveInput()
    {
        if (isGrounded)
        {
            if (moveInput != 0)
            {
                inputForce = isGrounded ? moveInput * forcesComponent.nominalForce : Vector3.zero;
                AccelerationPressed();
                if (velocity.x == 0) velocity = new Vector3(moveInput * movevelocity, velocity.y, 0.0f);
                totalForce += inputForce;
            }
            else
            {
                velocity = new Vector3(0.0f, velocity.y, 0.0f);
            }
        }
    }

    public Energy GetEnergy()
    {
        return energy;
    }
}