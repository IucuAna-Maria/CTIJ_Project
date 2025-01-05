using Platformer.Gameplay;
using Platformer.Mechanics;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using static Platformer.Core.Simulation;

public class SpikeDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Something collided with spikes!");

        var player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            //Debug.Log("Player collided with spikes!");

            //var health = player.GetComponent<Health>();
            //if (health != null)
            //{
            //    health.Decrement();
            //    Debug.Log("Player took damage!");
            //}
            player.health.Decrement();
            if (!player.health.IsAlive)
            {
                Schedule<PlayerDeath>();
            }
        }
    }
}
