using Platformer.Gameplay;
using Platformer.Mechanics;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using static Platformer.Core.Simulation;

public class BatDamage : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();

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
