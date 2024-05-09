
using System.Linq.Expressions;
using UnityEngine;

public class Rebound : MonoBehaviour
{
    public float mass = 0.1f;

    public float radius = 1.0f;

    public float gravity = 9.8f;

    public float stiffness = 0.8f;

    public Vector3 speed; // m/s
    private Vector3 acceleration;

    private bool isGrounded;

    private Vector3 weight;
    private Vector3 normalReaction;
    // Start is called before the first frame update

    void Start()
    {
        transform.localScale = new Vector3(2 * radius, 2 * radius, 0.0f);
        weight = new Vector3(0.0f, -mass * gravity, 0.0f);
        normalReaction = isGrounded ? weight : Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y - radius < 0)
        {
            transform.position += new Vector3(0.0f, radius - transform.position.y, 0.0f);
        }
        Vector3 F = weight + normalReaction;

        acceleration = F / mass;

        speed += acceleration * Time.fixedDeltaTime;
        transform.position += speed * Time.deltaTime;
        if (transform.position.y < radius)
        {
            speed.y = -stiffness * speed.y;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }


    }
}
