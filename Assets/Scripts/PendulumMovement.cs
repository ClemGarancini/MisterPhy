using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PendulumMovement : MonoBehaviour
{
    //private MecanicForces forcesComponent;
    private Tension tension;

    public float basketMass = 0.1f;
    private GravityField gravityField;

    public Vector3 tensionForce;
    private Vector3 weight;

    private float angle;
    private float initialAngle;

    public Vector3 velocity; // m/s
    public Vector3 acceleration;



    #region Player
    private Transform playerTransform;
    private PlayerController playerController;
    public GameObject player;

    #endregion

    private Transform thread;

    private Transform basis;

    public Transform basket;


    private Vector3 totalForce;
    private float length;
    private float gravity = 9.8f;

    void Start()
    {
        thread = transform.Find("Thread");
        basis = transform.Find("Basis");
        basket = transform.Find("Center");
        length = thread.localScale.y;

        tension = new(basis.position, length);
        gravityField = new(new Vector3(0.0f, -gravity, 0.0f));
        weight = gravityField.ComputeForce(basketMass);

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

        if (playerController != null && playerController.frameInput.jump)
        {
            PlayerOffPendulum();
        }
        else
        {

            Vector3 direction = (basket.position - basis.position).normalized;
            angle = Mathf.Atan2(direction.x, -direction.y);
            print(angle * Mathf.Rad2Deg);

            // Update pendulum rotation
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg);
            if (playerTransform != null) playerTransform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg);

            tensionForce = tension.ComputeForce(basket.position, angle, initialAngle, basketMass, gravity);

            totalForce = tensionForce + weight;
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