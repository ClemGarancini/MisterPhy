using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTeleport : MonoBehaviour
{
    public Transform respawn;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si un objet entre en collision avec le collider 2D
        if (respawn != null)
        {
            // Téléporter l'objet à la position du respawn
            float offsetX = other.transform.position.x - transform.position.x;
            other.transform.position = new Vector3(respawn.position.x + offsetX, respawn.position.y, 0.0f);
        }
    }
}
