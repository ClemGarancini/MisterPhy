using UnityEngine;
using System.Collections.Generic;
public class DrawArrow : MonoBehaviour
{
    public GameObject arrow;
    private PlayerController player;

    private GameObject weightArrow;
    private GameObject normalReactionArrow;
    private GameObject solidFrictionArrow;
    private GameObject fluidFrictionArrow;

    private List<GameObject> forcesArrow;

    private List<Vector3> externalForces;
    void Start()
    {
        player = GetComponent<PlayerController>();
        externalForces = player.externalForces;

        forcesArrow.Add(weightArrow);
        forcesArrow.Add(fluidFrictionArrow);
        forcesArrow.Add(normalReactionArrow);
        forcesArrow.Add(solidFrictionArrow);


        foreach (GameObject forceArrow in forcesArrow)
        {
            InstantiateArrow(forceArrow, GenerateRandomColor());
        }

    }
    void Update()
    {
        for (int i = 0; i < externalForces.Count; i++)
        {
            UpdateForceArrow(forcesArrow[i], transform.position, externalForces[i]);
        }
    }

    void InstantiateArrow(GameObject forceArrow, Color color)
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

    public Color GenerateRandomColor()
    {
        // Crée une instance de Random
        System.Random random = new();

        // Génère des valeurs aléatoires pour R, G et B entre 0 et 1
        float r = (float)random.NextDouble();
        float g = (float)random.NextDouble();
        float b = (float)random.NextDouble();

        // Retourne une nouvelle couleur avec les valeurs générées
        return new Color(r, g, b);
    }
}