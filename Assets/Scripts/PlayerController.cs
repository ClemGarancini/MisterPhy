
using UnityEngine;

using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Properties;
using System;
public class PlayerController : MonoBehaviour
{

    #region Shape characteristic
    public float radius = 1.0f;
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
    private Vector3 inputForce;
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
    public float maxVelocity = 20.0f;
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
        public bool isGroundedTemporary;
        public bool isGroundedPermanent;
        public HashSet<GameObject> collisionGameObjects;

    }

    public CollisionInformation collisionInformation;
    #endregion

    private void Start()
    {
        initialPosition = transform.position;

        forcesComponent = GetComponent<MecanicForces>();

        transform.localScale = new Vector3(2 * radius, 2 * radius, 0.0f);

        totalForce = Vector3.zero;

        energy = new();
        energy.Initialize();

        work = new();
    }

    private void Update()
    {

        totalForce = Vector3.zero;

        GetFrameInput();
        MoveInput();

        externalForces = forcesComponent.ComputeForces(this);
        gravityWork = work.GetWork(externalForces[0], transform.position - initialPosition);

        foreach (Vector3 force in externalForces)
        {
            totalForce += force; // Ajoutez chaque vecteur à la somme totale
        }


        // CheckGroundCollision();
        Jump();
        ComputeIntegration();

        kineticEnergy = energy.GetKineticEnergy(mass, velocity);
        gravitationalPotentialEnergy = energy.GetGravitationalPotentialEnergy(mass, -forcesComponent.gravity.y, transform.position.y - radius);
        totalEnergy = energy.GetTotalEnergy(mass, -forcesComponent.gravity.y, transform.position.y - radius, velocity);
    }

    private void ComputeIntegration()
    {
        float deltaTime = timeMultiplier * Time.deltaTime;
        acceleration = totalForce / mass;
        velocity += acceleration * deltaTime;
        transform.position += velocity * deltaTime;
    }

    private void AccelerationPressed()
    {
        if (frameInput.accelerate)
        {
            inputForce = 2 * frameInput.horizontal * forcesComponent.nominalForce;
        }
        VelocityLimit();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // print(velocity.y);

        Vector3 contactNormal = collision.GetContact(0).normal;
        float dotProd = Vector3.Dot(velocity, contactNormal);
        // print($"Vel: {velocity}, Norm: {contactNormal}, Dot:{dotProd}");
        if (!collisionInformation.isGroundedPermanent)
        {
            // print((1.0f + stiffness) * Math.Abs(dotProd) * contactNormal);
            velocity += (1.0f + stiffness) * Math.Abs(dotProd) * contactNormal;
            // velocity.y = -stiffness * velocity.y;
        }
        collisionInformation.isGroundedTemporary = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        collisionInformation.isGroundedPermanent = true;
        velocity.y = 0.0f;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        collisionInformation.isGroundedTemporary = false;
    }


    private void Jump()
    {
        if (frameInput.jump && collisionInformation.isGroundedPermanent)
        {
            velocity = new Vector3(velocity.x, jumpForce, 0.0f);
            collisionInformation.isGroundedPermanent = false;
        }
    }

    private void VelocityLimit()
    {
        if (Mathf.Abs(velocity.x) > maxVelocity)
        {
            velocity.x = maxVelocity * velocity.x / Mathf.Abs(velocity.x);
        }
    }

    private void MoveInput()
    {
        if (collisionInformation.isGroundedPermanent)
        {
            if (frameInput.horizontal != 0)
            {
                inputForce = frameInput.horizontal * forcesComponent.nominalForce;
                AccelerationPressed();
                if (velocity.x == 0) velocity = new Vector3(frameInput.horizontal * moveVelocity, velocity.y, 0.0f);
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