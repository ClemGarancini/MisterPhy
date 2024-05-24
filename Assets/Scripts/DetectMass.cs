using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetectMass : MonoBehaviour
{
    private Transform pendulum;
    private PendulumMovement pendulumMovement;

    void Start()
    {
        pendulum = transform.parent;
        pendulumMovement = pendulum.gameObject.GetComponent<PendulumMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        {
            pendulumMovement.player = other.gameObject;
            pendulumMovement.PlayerOnPendulum();
        }
    }

}