using UnityEngine;
using System.Collections.Generic;


public class DrawKinematic : MonoBehaviour
{
    private GameObject velocityArrow;
    private GameObject accelerationArrow;
    private GameObject velocityText;
    private GameObject accelerationText;

    public void Initialize(GameObject kinematicArrow, GameObject text)
    {

        velocityArrow = Instantiate(kinematicArrow, new Vector3(20.0f, -8.0f, 0), Quaternion.identity);
        velocityArrow.transform.localScale = new Vector3(1.0f, 5.0f, 0.0f);

        accelerationArrow = Instantiate(kinematicArrow, new Vector3(5.0f, -8.0f, 0), Quaternion.identity);
        accelerationArrow.transform.localScale = new Vector3(1.0f, 5.0f, 0.0f);

        velocityText = Instantiate(text, new Vector3(20.0f, -7.0f, 0), Quaternion.identity);
        velocityText.GetComponent<TextMesh>().text = "Velocity";

        accelerationText = Instantiate(text, new Vector3(5.0f, -7.0f, 0), Quaternion.identity);
        accelerationText.GetComponent<TextMesh>().text = "Acceleration";
    }
    public void Draw(Vector3 velocity, Vector3 acceleration)
    {

        //velocityArrow.transform.position = new Vector3(kineticSquare.transform.position.x, 5.0f + kineticSquare.transform.localScale.y / 2, 0.0f);
        velocityArrow.transform.localScale = new Vector3(velocity.magnitude / 5.0f, velocityArrow.transform.localScale.y, 0.0f);
        ComputeRotation(ref velocityArrow, velocity);

        accelerationArrow.transform.localScale = new Vector3(acceleration.magnitude / 5.0f, accelerationArrow.transform.localScale.y, 0.0f);
        ComputeRotation(ref accelerationArrow, acceleration);
        //accelerationArrow.transform.position = new Vector3(potentialSquare.transform.position.x, 5.0f + potentialSquare.transform.localScale.y / 2, 0.0f);

    }
    private void ComputeRotation(ref GameObject arrow, Vector3 vector)
    {
        float angle;
        if (vector.magnitude > 0)
        {
            if (vector.x != 0)
            {
                angle = 180f * Mathf.Atan2(vector.y, vector.x) / Mathf.PI;
            }
            else
            {
                angle = (vector.y > 0) ? 90f : -90f;
            }
            arrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }
    }

}