
using UnityEngine;

using System.Collections.Generic;
using System;
using UnityEngine.Animations;

public class GravityField : MonoBehaviour
{
    // The weight is a conservative force which means it derivates from a potential energy (the gravitational potential energy)
    private Vector3 gravity;

    public GravityField(Vector3 gravity)
    {
        this.gravity = gravity;
    }

    public Vector3 ComputeForce(float mass)
    {
        return mass * gravity;
    }
}

public class FluidFriction : MonoBehaviour
{
    private float dynamicViscosity;

    public FluidFriction(float dynamicViscosity)
    {
        this.dynamicViscosity = dynamicViscosity;
    }

    public Vector3 ComputeForce(float diameter, Vector3 velocity)
    {
        return -3 * Mathf.PI * dynamicViscosity * diameter * velocity;
    }
}

public class NormalReaction : MonoBehaviour
{
    public NormalReaction() { }

    public Vector3 ComputeForce(PlayerController.CollisionInformation collisionInformation, Vector3 weight)
    {
        Vector3 force = Vector3.zero;
        if (collisionInformation.isGrounded)
        {
            Vector3 contactNormal = collisionInformation.collision.GetContact(0).normal;
            force = Math.Abs(Vector3.Dot(contactNormal, weight)) * contactNormal;
        }
        return force;
    }
}

public class SolidFrictionDynamic : MonoBehaviour
{
    private float dynamicFrictionCoefficient;
    private float velocityMinThreshold;
    public SolidFrictionDynamic(float dynamicFrictionCoefficient)
    {
        this.dynamicFrictionCoefficient = dynamicFrictionCoefficient;
        this.velocityMinThreshold = 0.3f;
    }

    public Vector3 ComputeForce(PlayerController.CollisionInformation collisionInformation, PlayerController.FrameInput frameInput, ref Vector3 velocity, float maxVelocity)
    {
        if (collisionInformation.isGrounded)
        {
            float dotProd = Vector3.Dot(velocity, collisionInformation.tangent);
            if (frameInput.horizontal == 0 && Math.Abs(dotProd) <= velocityMinThreshold)
            {
                velocity -= dotProd * collisionInformation.tangent;
                return Vector3.zero;
            }
            else if (frameInput.horizontal == 0 || Math.Abs(dotProd) >= maxVelocity)
            {
                return -Math.Sign(dotProd) * dynamicFrictionCoefficient * collisionInformation.tangent;
            }
        }
        return Vector3.zero;
    }
}

public class InputImpulsion : MonoBehaviour
{
    private float dynamicFrictionCoefficient;
    public InputImpulsion(float dynamicFrictionCoefficient)
    {
        this.dynamicFrictionCoefficient = dynamicFrictionCoefficient;
    }

    public Vector3 ComputeForce(PlayerController.CollisionInformation collisionInformation, PlayerController.FrameInput frameInput)
    {
        if (collisionInformation.isGrounded)
        {
            return frameInput.horizontal * dynamicFrictionCoefficient * collisionInformation.tangent;
        }
        return Vector3.zero;
    }
}

public class MecanicForces : MonoBehaviour
{

    #region Constants
    private Vector3 gravity = new(0.0f, -9.8f, 0.0f);
    private double dynamicViscosity = 18.5 * 1e-6;
    private float dynamicFrictionCoefficient = 5.0f;
    #endregion

    #region Forces
    GravityField gravityField;
    private Vector3 weight;

    NormalReaction normalReaction;
    private Vector3 normalReactionForce;

    FluidFriction fluidFriction;
    private Vector3 fluidFrictionForce;


    SolidFrictionDynamic solidFrictionDynamic;
    private Vector3 solidFrictionDynamicForce;

    InputImpulsion inputImpulsion;
    private Vector3 inputImpulsionForce;

    #endregion

    void Start()
    {
        gravityField = new GravityField(gravity);
        normalReaction = new NormalReaction();

        fluidFriction = new((float)dynamicViscosity);

        solidFrictionDynamic = new SolidFrictionDynamic(dynamicFrictionCoefficient);
        inputImpulsion = new InputImpulsion(dynamicFrictionCoefficient);
    }

    // Update is called once per frame
    public List<Vector3> ComputeForces(PlayerController playerController)
    {
        float mass = playerController.mass;
        float radius = playerController.radius;
        Vector3 velocity = playerController.velocity;
        float maxVelocity = playerController.maxVelocity;
        PlayerController.CollisionInformation collisionInformation = playerController.collisionInformation;
        PlayerController.FrameInput frameInput = playerController.frameInput;

        List<Vector3> forces = new();

        weight = gravityField.ComputeForce(mass);
        forces.Add(weight);
        // print($"w: {weight}");

        fluidFrictionForce = fluidFriction.ComputeForce(2 * radius, velocity);
        forces.Add(fluidFrictionForce);
        // print($"fF: {fluidForce}");

        normalReactionForce = normalReaction.ComputeForce(collisionInformation, weight);
        forces.Add(normalReactionForce);
        // print($"n: {normalReaction}");

        solidFrictionDynamicForce = solidFrictionDynamic.ComputeForce(collisionInformation, frameInput, ref playerController.velocity, maxVelocity);
        forces.Add(solidFrictionDynamicForce);
        // print($"sF: {solidFriction}");

        inputImpulsionForce = inputImpulsion.ComputeForce(collisionInformation, frameInput);
        forces.Add(inputImpulsionForce);

        return forces;
    }
}
