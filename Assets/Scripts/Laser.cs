
using UnityEngine;
using UnityEngine.WSA;

public class Laser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private LineRenderer firstRefractedLineRenderer;
    private LineRenderer secondRefractedLineRenderer;

    public int maxReflections = 3; // Nombre maximum de réflexions

    public int maxRefractions = 2;

    public float airRefractiveIndex = 1.0f;

    public float laserFrequency = 7.0f; // Exemple: fréquence du laser

    void Start()
    {
        lineRenderer = transform.Find("Laser").GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Créer un nouveau LineRenderer pour le rayon réfracté
        firstRefractedLineRenderer = new GameObject("FirstRefractedLaser").AddComponent<LineRenderer>();
        firstRefractedLineRenderer.startWidth = 0.1f;
        firstRefractedLineRenderer.endWidth = 0.1f;
        firstRefractedLineRenderer.material = new Material(lineRenderer.material);
        firstRefractedLineRenderer.startColor = lineRenderer.startColor;
        firstRefractedLineRenderer.endColor = lineRenderer.endColor;





        secondRefractedLineRenderer = new GameObject("SecondRefractedLaser").AddComponent<LineRenderer>();
        secondRefractedLineRenderer.startWidth = 0.1f;
        secondRefractedLineRenderer.endWidth = 0.1f;
        secondRefractedLineRenderer.material = new Material(lineRenderer.material);
        secondRefractedLineRenderer.startColor = lineRenderer.startColor;
        secondRefractedLineRenderer.endColor = lineRenderer.endColor;


    }

    void Update()
    {

        lineRenderer.positionCount = 1;
        firstRefractedLineRenderer.positionCount = 0;
        secondRefractedLineRenderer.positionCount = 0;

        ChangeLaserColor(airRefractiveIndex, lineRenderer);



        Vector2 laserStartPosition = transform.Find("LaserStart").position;
        lineRenderer.SetPosition(0, laserStartPosition);

        Vector2 currentPosition = laserStartPosition;
        Vector2 direction = transform.right;

        float currentRefractiveIndex = airRefractiveIndex;

        LaunchRay(currentPosition, direction, currentRefractiveIndex, lineRenderer, 0);

    }

    private void LaunchRay(Vector2 currentPosition, Vector2 direction, float currentRefractiveIndex, LineRenderer currentLineRenderer, int numRefraction)
    {

        for (int i = 0; i < maxReflections; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction);

            if (hit.collider != null)
            {
                // Position du point d'impact
                Vector2 hitPoint = hit.point;

                // Ajouter un petit décalage pour sortir le rayon du mur
                currentPosition = hitPoint + hit.normal * 0.01f;

                // Ajuster le nombre de points pour tracer le rayon réfléchi
                currentLineRenderer.positionCount++;
                currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, currentPosition);

                if (hit.collider.gameObject.CompareTag("Transparent"))
                {

                    // Vérifier si l'objet est réfractif
                    RefractiveMaterial refractiveMaterial = hit.collider.GetComponent<RefractiveMaterial>();
                    if (refractiveMaterial != null && numRefraction < maxRefractions)
                    {
                        numRefraction++;
                        if (numRefraction == 1)
                        {
                            // Calculer la direction réfractée
                            Vector2 refractedDirection = Refract(direction, hit.normal, currentRefractiveIndex, refractiveMaterial.refractiveIndex);
                            float newRefractiveIndex = refractiveMaterial.refractiveIndex;
                            firstRefractedLineRenderer.positionCount = 1;
                            firstRefractedLineRenderer.SetPosition(0, hitPoint - hit.normal * 0.01f);
                            ChangeLaserColor(newRefractiveIndex, firstRefractedLineRenderer);

                            // Lancer le rayon réfracté
                            LaunchRay(hitPoint - hit.normal * 0.01f, refractedDirection, newRefractiveIndex, firstRefractedLineRenderer, numRefraction);

                        }
                        else if (numRefraction == 2)
                        {
                            // Calculer la direction réfractée
                            Vector2 refractedDirection = Refract(direction, hit.normal, currentRefractiveIndex, airRefractiveIndex);
                            float newRefractiveIndex = airRefractiveIndex;
                            secondRefractedLineRenderer.positionCount = 1;
                            secondRefractedLineRenderer.SetPosition(0, hitPoint - hit.normal * 0.01f);
                            ChangeLaserColor(airRefractiveIndex, secondRefractedLineRenderer);



                            // Lancer le rayon réfracté
                            LaunchRay(hitPoint - hit.normal * 0.01f, refractedDirection, newRefractiveIndex, secondRefractedLineRenderer, numRefraction);

                        }
                    }

                    // Calculer la direction réfléchie en utilisant la loi de Descartes
                    direction = Vector2.Reflect(direction, hit.normal);

                }


                else if (hit.collider.gameObject.CompareTag("Switch"))
                {
                    print(hit.collider.gameObject.GetComponent<SpriteRenderer>().color);
                    print(currentLineRenderer.endColor);
                    if (hit.collider.gameObject.GetComponent<SpriteRenderer>().color.b == currentLineRenderer.endColor.b)
                        hit.collider.gameObject.GetComponent<SwitchAction>().On();
                    break;
                }

                else break;


            }
            else
            {
                currentLineRenderer.positionCount++;
                // Si aucun impact, tracer le rayon droit
                currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, currentPosition + direction * 5000);
                break;
            }
        }

    }

    private Vector2 Refract(Vector2 direction, Vector2 normal, float n1, float n2)
    {
        float cosI = -Vector2.Dot(normal, direction);
        float sinT2 = (n1 / n2) * (n1 / n2) * (1.0f - cosI * cosI);
        if (sinT2 > 1.0f) // Total internal reflection
        {
            return Vector2.Reflect(direction, normal);
        }
        float cosT = Mathf.Sqrt(1.0f - sinT2);
        return (n1 / n2) * direction + (cosI * (n1 / n2) - cosT) * normal;
    }

    private void ChangeLaserColor(float refractiveIndex, LineRenderer line)
    {
        // Utiliser f = nv / λ pour calculer λ, où v = c (vitesse de la lumière dans le vide)
        float speedOfLight = 3.0e8f;
        float wavelength = refractiveIndex * speedOfLight / (laserFrequency * 1e14f);
        Color newColor = WavelengthToColor(wavelength);
        print(wavelength);



        line.startColor = newColor;
        line.endColor = newColor;
    }

    private Color WavelengthToColor(float wavelength)
    {
        // Simplification pour convertir une longueur d'onde en couleur (en nm)
        // Cette fonction peut être améliorée pour des conversions plus précises

        float gamma = 0.8f;
        float R, G, B;
        float wavelengthNM = wavelength * 1e9f; // Convertir la longueur d'onde en nm

        if (wavelengthNM >= 380 && wavelengthNM < 440)
        {
            R = -(wavelengthNM - 440) / (440 - 380);
            G = 0.0f;
            B = 1.0f;
        }
        else if (wavelengthNM >= 440 && wavelengthNM < 490)
        {
            R = 0.0f;
            G = (wavelengthNM - 440) / (490 - 440);
            B = 1.0f;
        }
        else if (wavelengthNM >= 490 && wavelengthNM < 510)
        {
            R = 0.0f;
            G = 1.0f;
            B = -(wavelengthNM - 510) / (510 - 490);
        }
        else if (wavelengthNM >= 510 && wavelengthNM < 580)
        {
            R = (wavelengthNM - 510) / (580 - 510);
            G = 1.0f;
            B = 0.0f;
        }
        else if (wavelengthNM >= 580 && wavelengthNM < 645)
        {
            R = 1.0f;
            G = -(wavelengthNM - 645) / (645 - 580);
            B = 0.0f;
        }
        else if (wavelengthNM >= 645 && wavelengthNM <= 780)
        {
            R = 1.0f;
            G = 0.0f;
            B = 0.0f;
        }
        else
        {
            R = 0.0f;
            G = 0.0f;
            B = 0.0f;
        }

        R = Mathf.Pow(R, gamma);
        G = Mathf.Pow(G, gamma);
        B = Mathf.Pow(B, gamma);
        return new Color(R, G, B);
    }

}
