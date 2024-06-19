
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class KineticEnergy
{
    public float GetKineticEnergy(float mass, Vector3 velocity)
    {
        return 0.5f * mass * Mathf.Pow(velocity.magnitude, 2);
    }
}

public class GravitationalPotentialEnergy
{
    public float GetGravitationalPotentialEnergy(float mass, float gravity, float height)
    {
        return mass * gravity * height;
    }
}

public class Energy
{
    private KineticEnergy kineticEnergy;
    private GravitationalPotentialEnergy gravitationalPotentialEnergy;

    public void Initialize()
    {
        kineticEnergy = new();
        gravitationalPotentialEnergy = new();
    }

    public float GetKineticEnergy(float mass, Vector3 velocity)
    {
        return kineticEnergy.GetKineticEnergy(mass, velocity);
    }


    public float GetGravitationalPotentialEnergy(float mass, float gravity, float height)
    {
        return gravitationalPotentialEnergy.GetGravitationalPotentialEnergy(mass, gravity, height);
    }

    public float GetTotalEnergy(float mass, float gravity, float height, Vector3 velocity)
    {
        return GetKineticEnergy(mass, velocity) + GetGravitationalPotentialEnergy(mass, gravity, height);
    }
}