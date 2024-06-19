using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [Header("Layers")]
    [Tooltip("Set this to the layer your player is on")]
    public LayerMask PlayerLayer;

    [Header("Dynamic characteristics")]
    [Tooltip("The mass of the player")]
    public float mass = 1.0f;

    [Tooltip("The radius of the player")]
    public float radius = 0.5f;

    [Tooltip("The stiffness of the player")]
    public float stiffness = 0.2f;

    [Header("Movement constants")]
    [Tooltip("The maximum velocity of the player along the slope")]
    public float maxVelocity = 7.0f;

    [Header("Jump force")]
    [Tooltip("The force at which the player will jump")]
    public float jumpForce = 5.0f;
}
