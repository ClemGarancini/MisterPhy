using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumMovement : MonoBehaviour
{
    //private MecanicForces forcesComponent;
    private Tension tension;

    public float mass = 1.0f;
    private GravityField gravityField;

    public Vector3 tensionForce;
    private Vector3 weight;

    private float angle;
    private float previousAngle;
    private float rotationSpeed;
    private float initialAngle;

    public Vector3 velocity; // m/s
    public Vector3 acceleration;

    public Transform obj;
    private Transform thread;

    private Transform basis;

    private Vector3 totalForce;
    private float length;
    private float gravity = 9.8f;
    void Start()
    {
        thread = transform.Find("Thread");
        basis = transform.Find("Basis");

        length = thread.localScale.y;

        tension = new(transform.position, length);
        gravityField = new(new Vector3(0.0f, -gravity, 0.0f));
        weight = gravityField.ComputeForce(mass);

        angle = Mathf.Acos(Mathf.Clamp((transform.position.y - obj.position.y) / length, -1.0f, 1.0f));
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * 180 / Mathf.PI);
        initialAngle = angle;
        previousAngle = angle;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = (obj.position - transform.position).normalized;
        angle = Mathf.Atan2(direction.x, -direction.y);
        // rotationSpeed = (angle - previousAngle) / Time.deltaTime;
        // previousAngle = angle;

        tensionForce = tension.ComputeForce(obj.position, angle, initialAngle, mass, gravity);
        totalForce = tensionForce + weight;
        ComputeIntegration();

        // Update pendulum rotation
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg);
        obj.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg);
    }

    private void ComputeIntegration()
    {
        acceleration = totalForce / mass;
        velocity += acceleration * Time.deltaTime;
        obj.position += velocity * Time.deltaTime;

    }
}
