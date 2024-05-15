using UnityEngine;
using System.Collections.Generic;

public class DrawEnergy : MonoBehaviour
{
    private GameObject kineticSquare;
    private GameObject kineticText;
    private GameObject potentialSquare;
    private GameObject potentialText;
    private GameObject totalSquare;
    private GameObject totalText;



    private Energy energy;
    public void Initialize(Energy e, GameObject energySquare, GameObject text)
    {
        energy = e;

        kineticSquare = Instantiate(energySquare, new Vector3(5.0f, 5.0f, 0), Quaternion.identity);
        kineticSquare.transform.localScale = new Vector3(1.0f, 5.0f, 0.0f);

        potentialSquare = Instantiate(energySquare, new Vector3(7.0f, 5.0f, 0), Quaternion.identity);
        potentialSquare.transform.localScale = new Vector3(1.0f, 5.0f, 0.0f);

        totalSquare = Instantiate(energySquare, new Vector3(9.0f, 5.0f, 0), Quaternion.identity);
        totalSquare.transform.localScale = new Vector3(1.0f, 5.0f, 0.0f);

        kineticText = Instantiate(text, new Vector3(5.0f, 4.0f, 0), Quaternion.identity);
        kineticText.GetComponent<TextMesh>().text = "EC";

        potentialText = Instantiate(text, new Vector3(7.0f, 4.0f, 0), Quaternion.identity);
        potentialText.GetComponent<TextMesh>().text = "EP";

        totalText = Instantiate(text, new Vector3(9.0f, 4.0f, 0), Quaternion.identity);
        totalText.GetComponent<TextMesh>().text = "E";
    }
    public void Draw(float kineticEnergy, float potentialEnergy, float totalEnergy)
    {
        kineticSquare.transform.localScale = new Vector3(1.0f, kineticEnergy / 10.0f, 0.0f);
        kineticSquare.transform.position = new Vector3(kineticSquare.transform.position.x, 5.0f + kineticSquare.transform.localScale.y / 2, 0.0f);

        potentialSquare.transform.localScale = new Vector3(1.0f, potentialEnergy / 10.0f, 0.0f);
        potentialSquare.transform.position = new Vector3(potentialSquare.transform.position.x, 5.0f + potentialSquare.transform.localScale.y / 2, 0.0f);

        totalSquare.transform.localScale = new Vector3(1.0f, totalEnergy / 10.0f, 0.0f);
        totalSquare.transform.position = new Vector3(totalSquare.transform.position.x, 5.0f + totalSquare.transform.localScale.y / 2, 0.0f);


    }

}