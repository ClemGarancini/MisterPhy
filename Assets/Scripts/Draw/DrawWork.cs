using UnityEngine;
using System.Collections.Generic;

public class DrawWork : MonoBehaviour
{
    private GameObject gravitySquare;
    private GameObject gravityText;

    public void Initialize(GameObject workSquare, GameObject text)
    {

        gravitySquare = Instantiate(workSquare, new Vector3(0.0f, 5.0f, 0), Quaternion.identity);
        gravitySquare.transform.localScale = new Vector3(1.0f, 5.0f, 0.0f);

        gravityText = Instantiate(text, new Vector3(0.0f, 4.0f, 0), Quaternion.identity);
        gravityText.GetComponent<TextMesh>().text = "Gravity Work";
    }
    public void Draw(float work)
    {
        gravitySquare.transform.localScale = new Vector3(1.0f, work / 10.0f, 0.0f);
        gravitySquare.transform.position = new Vector3(gravitySquare.transform.position.x, 5.0f + gravitySquare.transform.localScale.y / 2, 0.0f);

    }

}