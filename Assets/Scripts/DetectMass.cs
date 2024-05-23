using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectMass : MonoBehaviour
{
    private Transform pendulum;
    private PendulumMovement pendulumMovement;
    private PlayerController player;
    private Transform playerTransform;
    void Start()
    {
        pendulum = transform.parent;
        pendulumMovement = pendulum.gameObject.GetComponent<PendulumMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        {
            playerTransform = other.transform;
            pendulumMovement.obj = other.gameObject;
            pendulumMovement.enabled = true;
        }
    }

}