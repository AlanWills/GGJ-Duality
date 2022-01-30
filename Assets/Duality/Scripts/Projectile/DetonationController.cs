using Celeste.Events;
using Duality.Player;
using System.Collections;
using UnityEngine;

namespace Duality.Projectile
{
    public class DetonationController : MonoBehaviour
    {
        #region Properties and Fields
        
        [SerializeField] private ProjectileSettings projectileSettings;
        [SerializeField] private CircleCollider2D detonationRadius;
        [SerializeField] private Vector3Event onDetonated;
        [SerializeField] private AudioClipEvent playSFXOneShot;

        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            int paddleMask = -1;
            bool shouldDie = false;

            if (collision.CompareTag("Paddle"))
            {
                if (!projectileSettings.IsIgnoredByStatus || !collision.GetComponent<PaddleController>().PaddleState.HasStatus(projectileSettings.IgnoredByStatus))
                {
                    paddleMask = collision.GetComponent<PlayerMask>().Mask;
                }

                shouldDie = true;
            }
            else if (collision.CompareTag("Detonation"))
            {
                foreach (GameObject paddle in GameObject.FindGameObjectsWithTag("Paddle"))
                {
                    if (detonationRadius.Distance(paddle.GetComponent<Collider2D>()).isOverlapped &&
                        !(projectileSettings.IsIgnoredByStatus && paddle.GetComponent<PaddleController>().PaddleState.HasStatus(projectileSettings.IgnoredByStatus)))
                    {
                        paddleMask = paddle.GetComponent<PlayerMask>().Mask;
                    }
                }
                
                shouldDie = true;
            }

            if (paddleMask != -1)
            {
                projectileSettings.DetonatedNearPaddle(paddleMask);
            }

            if (shouldDie)
            {
                if (onDetonated != null)
                {
                    onDetonated.Invoke(transform.position);
                }

                if (projectileSettings.OnDetonateSFX != null)
                {
                    playSFXOneShot.Invoke(projectileSettings.OnDetonateSFX);
                }

                gameObject.SetActive(false);
            }
        }
    }
}