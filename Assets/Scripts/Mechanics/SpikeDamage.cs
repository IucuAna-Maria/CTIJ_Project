using Platformer.Gameplay;
using Platformer.Mechanics;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using static Platformer.Core.Simulation;

public class SpikeDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            player.health.TakeDamage(1);

            if (!player.health.IsAlive)
            {
                Schedule<PlayerDeath>();
            }
            else
            {
                player.Bounce(7);
            }
        }
    }
}
