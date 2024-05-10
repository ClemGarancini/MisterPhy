
using System.Collections.Generic;
using UnityEngine;

public class Rebound : MonoBehaviour
{
    public float mass = 0.1f;

    public float radius = 1.0f;

    public float gravity = 9.8f;

    public float stiffness = 0.8f;

    public Vector3 speed; // m/s
    public Vector3 acceleration;

    public bool isGrounded;

    private Vector3 weight;
    private Vector3 normalReaction;

    private Vector3 solidFriction;

    private Vector3 nominalForce = new Vector3(1.0f, 0.0f, 0.0f);
    private Vector3 inputForce;

    public float moveInput;
    // Start is called before the first frame update

    public GameObject arrow;
    private GameObject weightArrow;
    private GameObject normalReactionArrow;
    private GameObject accelerationArrow;

    private GameObject solidFrictionArrow;
    private GameObject inputForceArrow;


    //private List<Vector3> forcesList = new List<Vector3>();

    public float sprintMultiplier = 10.0f;
    void Start()
    {
        transform.localScale = new Vector3(2 * radius, 2 * radius, 0.0f);
        weight = new Vector3(0.0f, -mass * gravity, 0.0f);

        InstantiateArrow(ref weightArrow, Color.red);
        InstantiateArrow(ref normalReactionArrow, Color.blue);
        InstantiateArrow(ref solidFrictionArrow, Color.green);
        InstantiateArrow(ref inputForceArrow, Color.yellow);



    }

    // Update is called once per frame
    void Update()
    {

        normalReaction = isGrounded ? -weight : Vector3.zero;

        solidFriction = isGrounded ? -moveInput * nominalForce : Vector3.zero;

        inputForce = moveInput * nominalForce;

        if (transform.position.y - radius < 0)
        {
            transform.position += new Vector3(0.0f, radius - transform.position.y - 0.001f, 0.0f);
        }

        Vector3 F = weight + normalReaction + inputForce + solidFriction;


        acceleration = F / mass;


        if (Input.GetKey(KeyCode.LeftShift))
        {
            acceleration = new Vector3(sprintMultiplier, acceleration.y, 0.0f);
        }

        //Debug.Log("Acceleration: " + acceleration);
        //Debug.Log("Speed: " + speed);
        speed += acceleration * Time.deltaTime;
        transform.position += speed * Time.deltaTime;
        if (transform.position.y <= radius)
        {
            speed.y = -stiffness * speed.y;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        UpdateForceArrow(weightArrow, transform.position, weight);
        UpdateForceArrow(normalReactionArrow, transform.position, normalReaction); ;
        UpdateForceArrow(solidFrictionArrow, transform.position, solidFriction); ;
        UpdateForceArrow(inputForceArrow, transform.position, inputForce);



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
