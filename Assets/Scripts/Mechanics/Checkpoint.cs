using Platformer.Core;
using Platformer.Model;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Color inactiveColor = Color.red; // Initial color (inactive)
    public Color activeColor = Color.green; // Color when activated

    private SpriteRenderer spriteRenderer;
    private PlatformerModel model;

    private bool isActivated = false;  // Track if the checkpoint was activated

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = inactiveColor; // Set initial color
        }

        model = Simulation.GetModel<PlatformerModel>(); // Access the global model
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Checkpoint activated!");

            // Change the color of the checkpoint for visual feedback
            if (spriteRenderer != null)
            {
                spriteRenderer.color = activeColor;
            }

            // Flip the checkpoint on the X axis each time the player hits it
            if (!isActivated)
            {
                // Flip once when activated
                Vector3 currentScale = transform.localScale;
                transform.localScale = new Vector3(currentScale.x * -1, currentScale.y, currentScale.z);

                // Save the position of the checkpoint as the last spawn point
                if (model != null)
                {
                    model.lastCheckpointPosition = transform.position;
                }

                // Mark the checkpoint as activated to avoid flipping it again after first activation
                isActivated = true;
            }
        }
    }
}
