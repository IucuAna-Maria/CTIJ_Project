using Platformer.Mechanics;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    private Vector3 originalPosition; 
    private Collider2D objectCollider;
    private SpriteRenderer objectRenderer; 

    void Start()
    {
        originalPosition = transform.position;

        objectCollider = GetComponent<Collider2D>();
        objectRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
           
            player.health.AddHealth(1);

            objectCollider.enabled = false;
            objectRenderer.enabled = false;

            Invoke(nameof(Respawn), 5f);
        }
    }

    void Respawn()
    {
        transform.position = originalPosition;

        objectCollider.enabled = true;
        objectRenderer.enabled = true;
    }
}
