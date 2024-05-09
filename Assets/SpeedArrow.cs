using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedArrow : MonoBehaviour
{
    private Gravity gravity;
    public GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        gravity = GetComponent<Gravity>();

    }

    // Update is called once per frame
    void Update()
    {
        print(gravity.speed);
        Vector3 speed = gravity.speed;
        float scale = Mathf.Sqrt(Mathf.Pow(speed.x, 2) + Mathf.Pow(speed.y, 2));
        arrow.transform.localScale = new Vector3(scale, scale, 1.0f);
        float angle = 180f * Mathf.Atan2(speed.y, speed.x) / Mathf.PI;
        arrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }
}
