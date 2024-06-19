using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Work
{
    public float GetWork(Vector3 force, Vector3 displacement)
    {
        return Vector3.Dot(force, displacement);
    }

}