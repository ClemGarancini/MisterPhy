using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumController : MonoBehaviour
{
    //private MecanicForces forcesComponent;

    public float basketMass = 0.5f;


    public float angle;
    public float initialAngle;

    public Vector3 velocity; // m/s
    public Vector3 acceleration;

    #region Forces
    private MecanicForces forcesComponent;
    public List<Vector3> externalForces { get; private set; }
    Vector3 totalForce;
    #endregion

    #region Player
    private Transform playerTransform;
    private PlayerController playerController;
    public GameObject player;

    #endregion

    private Transform thread;

    public Transform basis;

    public Transform basket;

    public float length;

    void Start()
    {
        thread = transform.Find("Thread");
        basis = transform.Find("Basis");
        //basket = transform.Find("Center");
        length = (basket.position - basis.position).magnitude;


        forcesComponent = new();
        forcesComponent.Initialize("Pendulum");

        Vector3 direction = (basket.position - basis.position).normalized;
        angle = Mathf.Atan2(direction.x, -direction.y);
        initialAngle = angle;

    }
    public void PlayerOnPendulum()
    {
        playerController = player.GetComponent<PlayerController>();
        playerController.isOnPendulum = true;
        playerController.velocity = Vector3.zero;
        basketMass += playerController.mass;
        playerTransform = player.transform;
    }

    public void PlayerOffPendulum()
    {
        playerController.isOnPendulum = false;
        if (playerTransform != null) playerTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        player = null;
        playerController = null;
        playerTransform = null;
        basketMass -= playerController.mass;
    }
    void Update()
    {

        totalForce = Vector3.zero;
        if (playerController != null && playerController.frameInput.jump)
        {
            PlayerOffPendulum();
        }
        else
        {

            Vector3 direction = (basket.position - basis.position).normalized;
            angle = Mathf.Atan2(direction.x, -direction.y);
            //print(angle * Mathf.Rad2Deg);

            // Update pendulum rotation
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg);
            if (playerTransform != null) playerTransform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg);

            externalForces = forcesComponent.ComputeForces(this);
            foreach (Vector3 force in externalForces)
            {
                totalForce += force;
            }
            print(totalForce);
            ComputeIntegration();
        }

    }

    private void ComputeIntegration()
    {
        acceleration = totalForce / basketMass;
        velocity += acceleration * Time.deltaTime;
        basket.position += velocity * Time.deltaTime;
        if (playerTransform != null) playerTransform.position = basket.position;
        //EnforcePendulumLength();
    }

    private void EnforcePendulumLength()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        playerTransform.position -= ((playerTransform.position - transform.position).magnitude - length) * direction;
    }
}