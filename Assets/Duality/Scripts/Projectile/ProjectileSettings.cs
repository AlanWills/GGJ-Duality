using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Duality.Projectile
{
    [CreateAssetMenu(fileName = nameof(ProjectileSettings), menuName = "Duality/Projectile/Projectile Settings")]
    public class ProjectileSettings : ScriptableObject
    {
        #region Properties and Fields

        public bool DieOnHitPaddle => dieOnHitPaddle;

        public Sprite UISprite => uiSprite;

        [Header("Behaviour")]
        [SerializeField] private bool dieOnHitPaddle = false;

        [Header("Points")]
        [SerializeField] private int pointsOnCrossYourLine;
        [SerializeField] private int pointsOnCrossOpponentLine;
        [SerializeField] private int pointsOnHitYourPaddle;
        [SerializeField] private int pointsOnHitOpponentPaddle;

        [Header("Visuals")]
        [SerializeField] private Sprite uiSprite;

        [Header("Events")]
        [SerializeField] private ProjectileCommonEvents projectileCommonEvents;

        #endregion

        public void HitYourPaddle(int playerMask)
        {
            if (pointsOnHitYourPaddle != 0)
            {
                projectileCommonEvents.AddPoints(playerMask, pointsOnHitYourPaddle);
            }
        }

        public void HitOpponentsPaddle(int playerMask)
        {
            if (pointsOnHitOpponentPaddle != 0)
            {
                projectileCommonEvents.AddPoints(playerMask, pointsOnHitOpponentPaddle);
            }
        }

        public void CrossedYourLine(int playerMask)
        {
            if (pointsOnCrossYourLine != 0)
            {
                projectileCommonEvents.AddPoints(playerMask, pointsOnCrossYourLine);
            }
        }

        public void CrossedOpponentsLine(int playerMask)
        {
            if (pointsOnCrossOpponentLine != 0)
            {
                projectileCommonEvents.AddPoints(playerMask, pointsOnCrossOpponentLine);
            }
        }
    }
}