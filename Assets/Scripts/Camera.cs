using UnityEngine;
using System.Collections.Generic;

public class Camera : MonoBehaviour
{
    public GameObject target;
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 4.0f, -10.0f);
    }
}