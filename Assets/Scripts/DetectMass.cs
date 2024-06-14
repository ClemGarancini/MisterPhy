using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetectMass : MonoBehaviour
{
    private Transform pendulum;
    private PendulumController pendulumController;

    void Start()
    {
        pendulum = transform.parent;
        pendulumController = pendulum.gameObject.GetComponent<PendulumController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        {
            pendulumController.player = other.gameObject;
            pendulumController.PlayerOnPendulum();
        }
    }

}