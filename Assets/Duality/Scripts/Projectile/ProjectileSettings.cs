using Celeste.Events;
using Celeste.Tools.Attributes.GUI;
using Duality.Player;
using UnityEngine;

namespace Duality.Projectile
{
    [CreateAssetMenu(fileName = nameof(ProjectileSettings), menuName = "Duality/Projectile/Projectile Settings")]
    public class ProjectileSettings : ScriptableObject
    {
        #region Properties and Fields

        public float ExtraFiringSpeed => extraFiringSpeed;
        public float BounceIncrease => bounceIncrease;

        public bool DieOnHitPaddle => dieOnHitPaddle;
        public bool AppliesStatus => appliesStatus;
        public PaddleStatus StatusToApply => statusToApply;
        public float SecondsToApplyFor => secondsToApplyFor;
        public bool IsIgnoredByStatus => isIgnoredByStatus;
        public PaddleStatus IgnoredByStatus => ignoredByStatus;

        public Sprite UISprite => uiSprite;
        public AudioClip OnBounceSFX => onBounceSFX;
        public AudioClip OnDetonateSFX => onDetonateSFX;
        public AudioClip OnHitPaddleSFX => onHitPaddleSFX;
        public AudioClip OnCrossedLineSFX => onCrossedLineSFX;

        [Header("Behaviour")]
        [SerializeField] private float extraFiringSpeed = 0;
        [SerializeField] private float bounceIncrease = 0.2f;
        [SerializeField] private bool dieOnHitPaddle = false;
        [SerializeField] private bool appliesStatus = false;
        [SerializeField, ShowIf(nameof(appliesStatus))] private PaddleStatus statusToApply = PaddleStatus.None;
        [SerializeField, ShowIf(nameof(appliesStatus))] private float secondsToApplyFor = 3;
        [SerializeField] private bool isIgnoredByStatus = false;
        [SerializeField, ShowIf(nameof(isIgnoredByStatus))] private PaddleStatus ignoredByStatus = PaddleStatus.None;

        [Header("Points")]
        [SerializeField] private int yourPointsIfCrossesYourLine;
        [SerializeField] private int yourPointsIfCrossesOpponentLine;
        [SerializeField] private int yourPointsIfHitsYourPaddle;
        [SerializeField] private int yourPointsIfHitsOpponentPaddle;
        [SerializeField] private int opponentPointsIfCrossesYourLine;
        [SerializeField] private int opponentPointsIfCrossesOpponentLine;
        [SerializeField] private int opponentPointsIfHitsYourPaddle;
        [SerializeField] private int opponentPointsIfHitsOpponentPaddle;
        [SerializeField] private int opponentPointsIfDetonatesNearOpponentPaddle;
        [SerializeField] private int pointsIfDetonatesNearPaddle;

        [Header("Visuals")]
        [SerializeField] private Sprite uiSprite;

        [Header("Audio")]
        [SerializeField] private AudioClip onBounceSFX;
        [SerializeField] private AudioClip onDetonateSFX;
        [SerializeField] private AudioClip onHitPaddleSFX;
        [SerializeField] private AudioClip onCrossedLineSFX;

        [Header("Events")]
        [SerializeField] private ProjectileCommonEvents projectileCommonEvents;

        #endregion

        public void HitYourPaddle(int playerMask, int opponentMask)
        {
            if (yourPointsIfHitsYourPaddle != 0)
            {
                projectileCommonEvents.AddPoints(playerMask, yourPointsIfHitsYourPaddle);
            }

            if (opponentPointsIfHitsYourPaddle != 0)
            {
                projectileCommonEvents.AddPoints(opponentMask, opponentPointsIfHitsYourPaddle);
            }
        }

        public void HitOpponentsPaddle(int playerMask, int opponentMask)
        {
            if (yourPointsIfHitsOpponentPaddle != 0)
            {
                projectileCommonEvents.AddPoints(playerMask, yourPointsIfHitsOpponentPaddle);
            }

            if (opponentPointsIfHitsOpponentPaddle != 0)
            {
                projectileCommonEvents.AddPoints(opponentMask, opponentPointsIfHitsOpponentPaddle);
            }
        }

        public void CrossedYourLine(int playerMask, int opponentMask)
        {
            if (yourPointsIfCrossesYourLine != 0)
            {
                projectileCommonEvents.AddPoints(playerMask, yourPointsIfCrossesYourLine);
            }

            if (opponentPointsIfCrossesYourLine != 0)
            {
                projectileCommonEvents.AddPoints(opponentMask, opponentPointsIfCrossesYourLine);
            }
        }

        public void CrossedOpponentsLine(int playerMask, int opponentMask)
        {
            if (yourPointsIfCrossesOpponentLine != 0)
            {
                projectileCommonEvents.AddPoints(playerMask, yourPointsIfCrossesOpponentLine);
            }

            if (opponentPointsIfCrossesOpponentLine != 0)
            {
                projectileCommonEvents.AddPoints(opponentMask, opponentPointsIfCrossesOpponentLine);
            }
        }

        public void DetonatedNearPaddle(int paddleMask)
        {
            if (pointsIfDetonatesNearPaddle != 0)
            {
                projectileCommonEvents.AddPoints(paddleMask, pointsIfDetonatesNearPaddle);
            }
        }
    }
}