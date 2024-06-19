
using UnityEngine;

using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Properties;
using System;

public class PlayerController : MonoBehaviour
{

    #region Shape characteristic
    public float radius = 0.5f;
    public float mass = 1f;
    public float stiffness = 0.2f;
    #endregion

    #region Kinematic features
    public Vector3 velocity; // m/s
    public Vector3 acceleration;
    public float timeMultiplier = 1.0f;
    public Vector3 initialPosition;
    #endregion

    #region Forces
    private MecanicForces forcesComponent;
    public List<Vector3> externalForces { get; private set; }
    Vector3 totalForce;
    #endregion

    #region Energy
    private Energy energy;
    public float totalEnergy;
    public float kineticEnergy;
    public float gravitationalPotentialEnergy;

    #endregion

    #region Input features
    public float moveVelocity = 10f;
    public float jumpForce = 10f;
    public float maxVelocity = 10.0f;
    public bool allowRebound = false;
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
    #endregion

    public bool isOnPendulum = false;

    private void Start()
    {
        initialPosition = transform.position;

        forcesComponent = new();
        forcesComponent.Initialize("Player");

        transform.localScale = new Vector3(2 * radius, 2 * radius, 0.0f);

        energy = new Energy();
        energy.Initialize();

        work = new();
    }

    private void FixedUpdate()
    {

        totalForce = Vector3.zero;

        GetFrameInput();
        PFD();

        // if (Vector3.Distance(totalForce, new Vector3(0.0f, -9.8f, 0.0f)) > 0.01f)
        // {
        //     print($"Total Force: {totalForce}");
        // }


        Jump();

        kineticEnergy = energy.GetKineticEnergy(mass, velocity);
        // gravitationalPotentialEnergy = energy.GetGravitationalPotentialEnergy(mass, -forcesComponent.gravity.y, transform.position.y - radius);

    }


    private void PFD()
    {
        externalForces = forcesComponent.ComputeForces(this);
        foreach (Vector3 force in externalForces)
        {
            totalForce += force;
        }
        ComputeIntegration();
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

        if (allowRebound && dotProd > 0.5f)
        {
            velocity += (1.0f + stiffness) * Math.Abs(dotProd) * contactNormal;
        }
        else
        {
            velocity += dotProd * contactNormal;

            collisionInformation.isGrounded = true;
            collisionInformation.collision = collision;
            collisionInformation.normal = contactNormal;
            collisionInformation.tangent = Vector3.Cross(contactNormal, Vector3.forward).normalized;

            if (Vector3.Dot(contactNormal, Vector3.up) < 0)
            {
                collisionInformation.isGrounded = false;
            }
        }
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
            //collisionInformation.isGrounded = false;
        }
    }


    public Energy GetEnergy()
    {
        return energy;
    }

    void GetFrameInput()
    {
        frameInput = new FrameInput
        {
            horizontal = Input.GetAxisRaw("Horizontal"),
            jump = Input.GetKey(KeyCode.Space),
            accelerate = Input.GetKey(KeyCode.LeftShift)
        };
    }

}