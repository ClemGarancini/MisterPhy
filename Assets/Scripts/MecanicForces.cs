
using UnityEngine;

using System.Collections.Generic;

public class GravityField : MonoBehaviour
{
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

public class SolidFriction : MonoBehaviour
{
    private float dynamicFrictionCoef;

    public SolidFriction(float dynamicFrictionCoef)
    {
        this.dynamicFrictionCoef = dynamicFrictionCoef;
    }

    // public Vector3 ComputeForce()
}

public class MecanicForces : MonoBehaviour
{

    public Vector3 gravity = new(0.0f, -9.8f, 0.0f);


    private Vector3 weight;
    private Vector3 normalReaction;

    private Vector3 solidFriction;

    public Vector3 nominalForce = new(1.0f, 0.0f, 0.0f);

    private Vector3 fluidForce;



    private double dynamicViscosity = 18.5 * 1e-6;


    GravityField gravityField;
    FluidFriction fluidFriction;
    //private List<Vector3> forcesList = new List<Vector3>();

    void Start()
    {
        gravityField = new(gravity);
        fluidFriction = new((float)dynamicViscosity);

    }

    // Update is called once per frame
    public List<Vector3> ComputeForces(PlayerController playerController)
    {
        float mass = playerController.mass;
        float radius = playerController.radius;
        Vector3 velocity = playerController.velocity;
        PlayerController.CollisionInformation collisionInformation = playerController.collisionInformation;
        float moveInput = playerController.frameInput.horizontal;

        weight = gravityField.ComputeForce(mass);
        List<Vector3> forces = new() { weight };

        fluidForce = fluidFriction.ComputeForce(2 * radius, velocity);
        forces.Add(fluidForce);

        normalReaction = collisionInformation.isGroundedTemporary ? -weight : Vector3.zero;
        forces.Add(normalReaction);

        solidFriction = collisionInformation.isGroundedPermanent ? -moveInput * nominalForce : Vector3.zero;
        forces.Add(solidFriction);


        return forces;

    }
}
