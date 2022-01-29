using Celeste.Tools;
using Celeste.UI.Layout;
using Celeste.Utils;
using Duality.Player;
using System.Collections;
using UnityEngine;

namespace Duality.Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private ProjectileSettings projectileSettings;
        [SerializeField] private SpriteRenderer projectileSpriteRenderer;
        [SerializeField] private Rigidbody2D projectileRigidbody;

        private int playerMask;
        private int opponentMask;

        #endregion

        public void Hookup(
            Vector3 position, 
            Quaternion rotation, 
            float forwardVelocity, 
            int playerMask,
            int opponentMask)
        {
            this.playerMask = playerMask;
            this.opponentMask = opponentMask;

            transform.position = position;
            transform.rotation = rotation;
            transform.Translate(new Vector3(0, projectileSpriteRenderer.sprite.bounds.extents.magnitude * projectileSpriteRenderer.transform.lossyScale.y));
            gameObject.SetActive(true);
            projectileRigidbody.velocity = transform.TransformVector(new Vector2(0, forwardVelocity));
        }

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref projectileSpriteRenderer);
            this.TryGet(ref projectileRigidbody);
        }

        private void OnDisable()
        {
            projectileRigidbody.velocity = Vector2.zero;
            playerMask = -1;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (projectileRigidbody.velocity.sqrMagnitude < 0.1f)
            {
                // Do not do collision handling if the projectile is not moving
                return;
            }

            if (collision.CompareTag("Paddle"))
            {
                PaddleState paddleState = collision.GetComponent<PaddleController>().PaddleState;
                int collisionMask = collision.GetComponent<PlayerMask>().Mask;
                Debug.Assert(playerMask != -1, $"Player Mask as been unset - did you deactivate the game object before resolving a collision?");

                if (collisionMask == playerMask)
                {
                    projectileSettings.HitYourPaddle(playerMask, opponentMask);
                }
                else
                {
                    projectileSettings.HitOpponentsPaddle(playerMask, opponentMask);
                }

                bool appliesStatus = projectileSettings.AppliesStatus;
                bool paddleHasStatus = appliesStatus && paddleState.HasStatus(projectileSettings.StatusToApply);

                // We should only die if we've marked we should die on paddle hit and we don't apply a status, or the paddle does not have the status
                bool shouldDie = projectileSettings.DieOnHitPaddle && (!appliesStatus || !paddleHasStatus);

                if (appliesStatus && !paddleHasStatus)
                {
                    paddleState.AddStatus(projectileSettings.StatusToApply, projectileSettings.SecondsToApplyFor);
                }

                if (shouldDie)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    projectileRigidbody.velocity *= new Vector2(-1, 1);
                }
            }
            else if (collision.CompareTag("Lines"))
            {
                int collisionMask = collision.GetComponent<PlayerMask>().Mask;
                Debug.Assert(playerMask != -1, $"Player Mask as been unset - did you deactivate the game object before resolving a collision?");

                if (collisionMask == playerMask)
                {
                    projectileSettings.CrossedYourLine(playerMask, opponentMask);
                }
                else
                {
                    projectileSettings.CrossedOpponentsLine(playerMask, opponentMask);
                }

                gameObject.SetActive(false);
            }
            else if (collision.CompareTag("Bounds"))
            {
                projectileRigidbody.velocity *= new Vector2(1, -1);
            }
        }

        #endregion
    }
}