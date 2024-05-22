using UnityEngine;
using System.Collections.Generic;


public class DrawForce : MonoBehaviour
{
    private GameObject forceArrow;

    public void Initialize(GameObject arrow, Transform obj)
    {

        forceArrow = Instantiate(arrow, obj.position, Quaternion.identity);
        forceArrow.transform.localScale = new Vector3(1.0f, 5.0f, 0.0f);

    }
    public void Draw(Vector3 force, Transform obj)
    {

        forceArrow.transform.position = obj.position;
        forceArrow.transform.localScale = new Vector3(force.magnitude / 5.0f, forceArrow.transform.localScale.y, 0.0f);
        ComputeRotation(ref forceArrow, force);

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