using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PendulumMovement : MonoBehaviour
{
    //private MecanicForces forcesComponent;
    private Tension tension;

    public float mass;
    private GravityField gravityField;

    public Vector3 tensionForce;
    private Vector3 weight;

    private float angle;
    private float initialAngle;

    public Vector3 velocity; // m/s
    public Vector3 acceleration;



    private Transform objTransform;
    private PlayerController objController;
    public GameObject obj;



    private Transform thread;

    private Transform basis;

    public Transform center;


    private Vector3 totalForce;
    private float length;
    private float gravity = 9.8f;

    void Start()
    {
        thread = transform.Find("Thread");
        basis = transform.Find("Basis");
        center = transform.Find("Center");
        length = thread.localScale.y;

        tension = new(transform.position, length);
        gravityField = new(new Vector3(0.0f, -gravity, 0.0f));
        weight = gravityField.ComputeForce(mass);
    }
    void OnEnable()
    {
        objController = obj.GetComponent<PlayerController>();
        objController.velocity = Vector3.zero;
        mass = objController.mass;
        objTransform = obj.transform;

        Vector3 direction = (objTransform.position - transform.position).normalized;
        angle = Mathf.Atan2(direction.x, -direction.y);
        //transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * 180 / Mathf.PI);
        initialAngle = angle;
    }

    void OnDisable()
    {
        if (objTransform != null) objTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        obj = null;
        objController = null;
        objTransform = null;
        mass = 0.0f;
        velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
    void Update()
    {

        if (objController.frameInput.jump)
        {
            enabled = false;
        }
        else
        {

            Vector3 direction = (objTransform.position - transform.position).normalized;
            angle = Mathf.Atan2(direction.x, -direction.y);


            tensionForce = tension.ComputeForce(objTransform.position, angle, initialAngle, mass, gravity);
            totalForce = tensionForce + weight;
            ComputeIntegration();

            // Update pendulum rotation
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg);
            objTransform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg);
        }

    }

    private void ComputeIntegration()
    {
        acceleration = totalForce / mass;
        velocity += acceleration * Time.deltaTime;
        objTransform.position += velocity * Time.deltaTime;
        EnforcePendulumLength();
    }

    private void EnforcePendulumLength()
    {
        Vector3 direction = (objTransform.position - transform.position).normalized;
        objTransform.position -= ((objTransform.position - transform.position).magnitude - length) * direction;
    }
}
