using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using System;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 5f;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7f;

        // For double-jump
        private int jumpCount = 0;            // Numarul de salturi efectuate
        private const int maxJumpCount = 2;   // Maximul de salturi permise (double jump)

        // ---------------------- DASH ----------------------
        [Header("Dash Settings")]
        public float dashSpeed = 10f;         // Viteza pe durata dash-ului
        public float dashDuration = 0.5f;       // Durata in secunde a dash-ului

        private float dashTimeRemaining;      // Cronometru pentru dash
        private bool isDashing;               // Indica daca jucatorul este in dash
        private bool usedAirDash;             // Indica daca s-a folosit dash-ul in aer

        public JumpState jumpState = JumpState.Grounded;
        /*internal new*/
        public Collider2D collider2d;
        /*internal new*/
        public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

       

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");

                // Allow jumping if the player is grounded or has jumps left
                if (Input.GetButtonDown("Jump"))
                {
                    if (jumpState == JumpState.Grounded || jumpCount < maxJumpCount)
                    {
                        jumpState = JumpState.PrepareToJump;
                    }
                }

                // Dash la apasarea tastei LeftShift (sau o alta tasta, dupa preferinte)
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    AttemptDash();
                }
            }
            else
            {
                move.x = 0;
            }

            // Actualizam starea de dash (daca este activ)
            UpdateDashState();

            // Actualizam starea de salt
            UpdateJumpState();

            base.Update();
        }

        /// <summary>
        /// Initiaza un dash daca jucatorul are dreptul (la sol sau nu a folosit inca dash-ul in aer).
        /// </summary>
        void AttemptDash()
        {
            // Verificam daca nu este deja in dash
            if (!isDashing)
            {
                // Poti da dash daca esti la sol...
                if (IsGrounded)
                {
                    StartDash();
                }
                else
                {
                    // ... sau daca nu ai folosit inca dash-ul in aer
                    if (!usedAirDash)
                    {
                        StartDash();
                        usedAirDash = true; // Marcam ca am folosit dash-ul in aer
                    }
                }
            }
        }

        /// <summary>
        /// Porneste efectiv dash-ul, setand cronometru si flag-ul de dash.
        /// </summary>
        void StartDash()
        {
            isDashing = true;
            dashTimeRemaining = dashDuration;
        }

        /// <summary>
        /// Actualizeaza starea de dash, reducand timpul ramas si resetand viteza la final.
        /// </summary>
        void UpdateDashState()
        {
            if (isDashing)
            {
                dashTimeRemaining -= Time.deltaTime;

                if (dashTimeRemaining <= 0)
                {
                    // S-a terminat dash-ul
                    isDashing = false;
                    // Revenim la viteza normala
                    maxSpeed = 5f;
                }
                else
                {
                    // Cat timp dureaza dash-ul, viteza e setata la dashSpeed
                    maxSpeed = dashSpeed;
                }
            }
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    if (jumpCount < maxJumpCount)
                    {
                        jumpState = JumpState.Jumping;
                        jump = true;
                        jumpCount++; // Increment the jump count
                    }
                    break;

                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;

                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;

                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    jumpCount = 0;
                    usedAirDash = false; // Resetam dash-ul in aer
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            // targetVelocity defineste viteza orizontala pe care ne-o dorim
            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        


    }
}