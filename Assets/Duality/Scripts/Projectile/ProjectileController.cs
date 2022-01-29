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

        #endregion

        public void Hookup(Vector3 position, Quaternion rotation, float forwardVelocity, int playerMask)
        {
            this.playerMask = playerMask;

            transform.position = position;
            transform.rotation = rotation;
            transform.Translate(new Vector3(0, projectileSpriteRenderer.sprite.bounds.extents.y));
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
                if (projectileSettings.DieOnHitPaddle)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    projectileRigidbody.velocity *= new Vector2(-1, 1);
                }

                int collisionMask = collision.GetComponent<PlayerMask>().Mask;
                if (collisionMask == playerMask)
                {
                    projectileSettings.HitYourPaddle(collisionMask);
                }
                else
                {
                    projectileSettings.HitOpponentsPaddle(collisionMask);
                }
            }
            else if (collision.CompareTag("Lines"))
            {
                gameObject.SetActive(false);

                int collisionMask = collision.GetComponent<PlayerMask>().Mask;
                if (collisionMask == playerMask)
                {
                    projectileSettings.CrossedYourLine(collisionMask);
                }
                else
                {
                    projectileSettings.CrossedOpponentsLine(collisionMask);
                }
            }
            else if (collision.CompareTag("Bounds"))
            {
                projectileRigidbody.velocity *= new Vector2(1, -1);
            }
        }

        #endregion
    }
}