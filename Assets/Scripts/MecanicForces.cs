
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

public class MecanicForces : MonoBehaviour
{

    public Vector3 gravity = new(0.0f, -9.8f, 0.0f);


    private Vector3 weight;
    private Vector3 normalReaction;

    private Vector3 solidFriction;

    public Vector3 nominalForce = new(1.0f, 0.0f, 0.0f);

    private Vector3 fluidForce;

    // Start is called before the first frame update

    public GameObject arrow;
    private GameObject weightArrow;
    private GameObject normalReactionArrow;

    private GameObject solidFrictionArrow;
    private GameObject inputForceArrow;
    private GameObject fluidFrictionArrow;


    private double dynamicViscosity = 18.5 * 1e-6;


    GravityField gravityField;
    FluidFriction fluidFriction;
    //private List<Vector3> forcesList = new List<Vector3>();

    void Start()
    {

        InstantiateArrow(ref weightArrow, Color.red);
        InstantiateArrow(ref normalReactionArrow, Color.blue);
        InstantiateArrow(ref solidFrictionArrow, Color.green);
        InstantiateArrow(ref fluidFrictionArrow, Color.grey);


        gravityField = new(gravity);
        fluidFriction = new((float)dynamicViscosity);

    }

    // Update is called once per frame
    public List<Vector3> ComputeForces(float mass, float radius, Vector3 speed, bool isGrounded, float moveInput)
    {
        weight = gravityField.ComputeForce(mass);
        List<Vector3> forces = new() { weight };

        fluidForce = fluidFriction.ComputeForce(2 * radius, speed);
        forces.Add(fluidForce);

        normalReaction = isGrounded ? -weight : Vector3.zero;
        forces.Add(normalReaction);

        solidFriction = isGrounded ? -moveInput * nominalForce : Vector3.zero;
        forces.Add(solidFriction);


        UpdateForceArrow(weightArrow, transform.position, weight);
        UpdateForceArrow(normalReactionArrow, transform.position, normalReaction);
        UpdateForceArrow(solidFrictionArrow, transform.position, solidFriction);
        UpdateForceArrow(fluidFrictionArrow, transform.position, fluidForce);

        return forces;

    }

    void InstantiateArrow(ref GameObject forceArrow, Color color)
    {

        forceArrow = Instantiate(arrow, transform.position, Quaternion.identity);
        GameObject square = forceArrow.transform.Find("Square").gameObject;
        GameObject triangle = forceArrow.transform.Find("Triangle").gameObject;
        SpriteRenderer squareRender = square.GetComponent<SpriteRenderer>();
        squareRender.color = color;
        SpriteRenderer triangleRender = triangle.GetComponent<SpriteRenderer>();
        triangleRender.color = color;
    }

    // Fonction pour dessiner les forces
    void UpdateForceArrow(GameObject forceArrow, Vector3 position, Vector3 force)
    {
        forceArrow.transform.position = position;
        float magnitude = Mathf.Sqrt(Mathf.Pow(force.x, 2) + Mathf.Pow(force.y, 2));
        forceArrow.transform.localScale = new Vector3(magnitude, 1.0f, 1.0f);

        float angle;

        if (magnitude > 0)
        {
            if (force.x != 0)
            {
                angle = 180f * Mathf.Atan2(force.y, force.x) / Mathf.PI;
            }
            else
            {
                angle = (force.y > 0) ? 90f : -90f;
            }
            forceArrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }




    }
}
