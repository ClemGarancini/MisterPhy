using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    private Vector3 gravity = new Vector3(0.0f, -9.8f, 0.0f); // m/s2
    private Vector3 speed; // m/s
    private Vector3 acceleration;
    private bool isGrounded;

    void Start()
    {
        speed = Vector3.zero;
        acceleration = Vector3.zero;
        isGrounded = false;
    }

    void Update()
    {
        acceleration = isGrounded ? Vector3.zero : gravity;
    }

    void FixedUpdate()
    {
        speed += acceleration * Time.fixedDeltaTime;
        transform.position += speed * Time.fixedDeltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Floor")
        {
            isGrounded = true;
            speed.y = 0.0f;
        }
    }
}
