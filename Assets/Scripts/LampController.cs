using UnityEngine;

using System.Collections.Generic;
using System;

public class LampController : MonoBehaviour
{
    public bool isOnHand = false;

    public PlayerController playerController;
    public GameObject player;
    public GameObject notificationIcon;

    private Laser laser;

    private float currentRotationZ = 0f;

    void Start()
    {
        laser = GetComponent<Laser>();
    }

    public void showNotificationIcon()
    {
        notificationIcon.SetActive(true);
    }
    public void hideNotificationIcon()
    {
        notificationIcon.SetActive(false);
    }

    void Update()
    {
        if (isOnHand)
        {
            if (playerController.velocity.x < -0.01f)
            {
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, currentRotationZ);
                transform.position = player.transform.Find("LampPositionLeft").position;
            }
            else if (playerController.velocity.x > 0.001f)
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, currentRotationZ); // Tourner vers la droite avec la rotation accumulée
                transform.position = player.transform.Find("LampPositionRight").position;
            }
            else
            {
                // Conserver la rotation accumulée lorsqu'il n'y a pas de mouvement horizontal
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, currentRotationZ);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {

                laser.enabled = !laser.enabled;
                if (!laser.enabled) laser.lineRenderer.positionCount = 0;
            }

            RotateLamp();
        }


    }

    private void RotateLamp()
    {
        // Ajouter à la rotation cumulée en fonction de l'input vertical
        float verticalInput = Input.GetAxis("Vertical");
        currentRotationZ += verticalInput * 100f * Time.deltaTime; // Ajuster 100f pour la vitesse de rotation

        // Appliquer la rotation accumulée
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, currentRotationZ);
    }

}