
using UnityEngine;

using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Properties;
using System;
public class PlayerController : MonoBehaviour
{

    public PlayerStats playerStats;
    #region Shape characteristic
    public float radius;
    public float mass;
    public float stiffness;
    #endregion

    #region Kinematic features
    public Vector3 velocity; // m/s
    public Vector3 acceleration;
    public float timeMultiplier = 1.0f;

    public Vector3 initialPosition;
    #endregion

    #region Forces
    private MecanicForces externalForcesComponent;
    private InternalForces internalForcesComponent;
    public List<Vector3> externalForces { get; private set; }
    public List<Vector3> internalForces { get; private set; }
    Vector3 totalForce;
    #endregion

    #region Energy
    private Energy energy;
    public float totalEnergy;
    public float kineticEnergy;
    public float gravitationalPotentialEnergy;

    #endregion

    #region Input features
    public float jumpForce;
    public float maxVelocity;
    #endregion

    #region Work

    private Work work;
    public float gravityWork;

    #endregion

    #region Input
    public struct FrameInput
    {
        public float horizontal;
        public bool jump;
        public bool accelerate;
    }
    public FrameInput frameInput { get; private set; }
    #endregion

    #region State
    public struct CollisionInformation
    {
        public bool isGrounded;
        public Collision2D collision;
        public Vector3 normal;
        public Vector3 tangent;
    }
    public CollisionInformation collisionInformation;

    public bool isOnPendulum = false;
    #endregion


    private void InitConstants()
    {
        mass = playerStats.mass;
        radius = playerStats.radius;
        stiffness = playerStats.stiffness;
        maxVelocity = playerStats.maxVelocity;
        jumpForce = playerStats.jumpForce;
    }
    private void Start()
    {
        InitConstants();
        initialPosition = transform.position;

        externalForcesComponent = GetComponent<MecanicForces>();
        internalForcesComponent = GetComponent<InternalForces>();

        transform.localScale = new Vector3(2 * radius, 2 * radius, 0.0f);

        totalForce = Vector3.zero;

        energy = new();
        energy.Initialize();

        work = new();
    }

    private void FixedUpdate()
    {

        totalForce = Vector3.zero;

        GetFrameInput();

        externalForces = externalForcesComponent.ComputeForces(this);
        internalForces = internalForcesComponent.ComputeForces(this);
        gravityWork = work.GetWork(externalForces[0], transform.position - initialPosition);

        foreach (Vector3 force in externalForces)
        {
            totalForce += force; // Ajoutez chaque vecteur à la somme totale
        }
        foreach (Vector3 force in internalForces)
        {
            totalForce += force; // Ajoutez chaque vecteur à la somme totale
        }

        // if (Vector3.Distance(totalForce, new Vector3(0.0f, -9.8f, 0.0f)) > 0.01f)
        // {
        //     print($"Total Force: {totalForce}");
        // }


        Jump();
        ComputeIntegration();

        kineticEnergy = energy.GetKineticEnergy(mass, velocity);
        // gravitationalPotentialEnergy = energy.GetGravitationalPotentialEnergy(mass, -forcesComponent.gravity.y, transform.position.y - radius);
        // totalEnergy = energy.GetTotalEnergy(mass, -forcesComponent.gravity.y, transform.position.y - radius, velocity);
    }

    private void ComputeIntegration()
    {
        float deltaTime = timeMultiplier * Time.deltaTime;
        acceleration = totalForce / mass;
        velocity += acceleration * deltaTime;
        transform.position += velocity * deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 contactNormal = collision.GetContact(0).normal;
        float dotProd = Math.Abs(Vector3.Dot(velocity, contactNormal));
        // Rebound
        // if (dotProd > 0.5f)
        // {
        //     print("dot " + dotProd);
        //     velocity += (1.0f + stiffness) * dotProd * contactNormal;
        // }
        // else

        velocity += dotProd * contactNormal;

        collisionInformation.isGrounded = true;
        collisionInformation.collision = collision;
        collisionInformation.normal = contactNormal;
        collisionInformation.tangent = Vector3.Cross(contactNormal, Vector3.forward).normalized;

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        collisionInformation.isGrounded = false;
    }

    private void Jump()
    {
        if (frameInput.jump && collisionInformation.isGrounded)
        {
            velocity = new Vector3(velocity.x, jumpForce, 0.0f);
            collisionInformation.isGrounded = false;
        }
    }

    // private void VelocityLimit()
    // {
    //     if (Mathf.Abs(velocity.x) > maxVelocity)
    //     {
    //         velocity.x = maxVelocity * velocity.x / Mathf.Abs(velocity.x);
    //     }
    // }

    public Energy GetEnergy()
    {
        return energy;
    }

    void GetFrameInput()
    {
        frameInput = new FrameInput
        {
            horizontal = Input.GetAxisRaw("Horizontal"),
            jump = Input.GetKeyDown(KeyCode.Space),
            accelerate = Input.GetKey(KeyCode.LeftShift)
        };
    }

}