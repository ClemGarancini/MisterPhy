using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey;
    //public UnityEvent interactAction;
    private bool notifyPlayerOnInteractible;

    private GameObject Player;

    private LampController lampController;

    void Start()
    {
        notifyPlayerOnInteractible = true;
        lampController = GetComponent<LampController>();
    }

    void Update()
    {
        if (isInRange)
        {
            if (Input.GetKey(KeyCode.A))
            {
                //interactAction.Invoke(); 
                notifyPlayerOnInteractible = false;
                transform.parent = Player.transform;
                lampController.isOnHand = true;
                lampController.playerController = Player.GetComponent<PlayerController>();
                lampController.player = Player;
                lampController.GetComponent<CapsuleCollider2D>().enabled = false;
                enabled = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (notifyPlayerOnInteractible)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                Player = collider.gameObject;
                isInRange = true;
                gameObject.GetComponent<LampController>().showNotificationIcon();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            gameObject.GetComponent<LampController>().hideNotificationIcon();
        }
    }
}
