using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 20f;

    private Rebound rebound;

    private void Start()
    {
        rebound = GetComponent<Rebound>();
    }

    private void Update()
    {
        // Déplacement horizontal
        float moveInput = Input.GetAxisRaw("Horizontal");
        rebound.speed = new Vector3(moveInput * moveSpeed, rebound.speed.y, 0.0f);
        rebound.moveInput = moveInput;




        // Saut
        if (Input.GetKeyDown(KeyCode.Space) && rebound.isGrounded)
        {
            rebound.speed = new Vector3(rebound.speed.x, jumpForce, 0.0f);
            rebound.isGrounded = false;
        }

    }
}