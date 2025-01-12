using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            player.collider2d.enabled = true;
            player.controlEnabled = false;

            if (player.audioSource && player.respawnAudio)
                player.audioSource.PlayOneShot(player.respawnAudio);

            // Asiguram ca folosim ultima pozitie a checkpoint-ului activ
            Vector3 respawnPosition = model.lastCheckpointPosition != Vector3.zero
                ? model.lastCheckpointPosition
                : model.spawnPoint.transform.position;

            // Repozitionam player-ul
            player.Teleport(respawnPosition);
            player.health.ResetHealth();
            player.jumpState = PlayerController.JumpState.Grounded;
            player.animator.SetBool("dead", false);

            model.virtualCamera.Follow = player.transform;
            model.virtualCamera.LookAt = player.transform;
            Simulation.Schedule<EnablePlayerInput>(2f);
        }
    }
}
