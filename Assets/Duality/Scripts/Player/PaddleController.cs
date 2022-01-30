using Celeste.Tools;
using Celeste.UI.Layout;
using Celeste.Utils;
using Duality.Events;
using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Duality.Player
{
    public class PaddleController : MonoBehaviour
    {
        #region Properties and Fields

        public PaddleState PaddleState => paddleState;

        [Header("Data")]
        [SerializeField] private PaddleSettings paddleSettings;
        [SerializeField] private PaddleState paddleState;

        [Header("Runtime")]
        [SerializeField] private Rigidbody2D paddleRigidbody2D;
        [SerializeField] private SpriteRenderer paddleSpriteRenderer;
        [SerializeField] private Transform aimDirection;
        [SerializeField] private SpriteRenderer aimDirectionSpriteRenderer;
        [SerializeField] private GameObject frozenEffectGameObject;
        [SerializeField] private GameObject shieldEffectGameObject;
        [SerializeField] private ReferenceLayout gameBounds;
        [SerializeField] private float angleMultiplier = 1;

        private bool moving;
        private bool aiming;
        private float currentAimAngle = 0;
        private Vector2 currentNormalizedVelocity;
        private Vector3 gameBoundsMinWorldSpace;
        private Vector3 gameBoundsMaxWorldSpace;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref paddleRigidbody2D);
            this.TryGet(ref paddleSpriteRenderer);
        }

        private void Awake()
        {
            currentAimAngle = 0;

            if (paddleSettings.OverridePaddleColour)
            {
                paddleSpriteRenderer.color = paddleSettings.PaddleColour;
                aimDirectionSpriteRenderer.color = paddleSettings.PaddleColour;
            }
        }

        private void Start()
        {
            ResetPaddle();

            Rect gameBoundsUISpace = gameBounds.rectTransform.GetWorldRect();
            gameBoundsMinWorldSpace = Camera.main.ScreenToWorldPoint(new Vector3(gameBoundsUISpace.xMin, gameBoundsUISpace.yMin, 0));
            gameBoundsMaxWorldSpace = Camera.main.ScreenToWorldPoint(new Vector3(gameBoundsUISpace.xMax, gameBoundsUISpace.yMax, 0));
        }

        private void Update()
        {
            if (aiming)
            {
                if (currentNormalizedVelocity.y != 0)
                {
                    float delta = Time.deltaTime * paddleSettings.AimSpeed * currentNormalizedVelocity.y;
                    currentAimAngle = Mathf.Clamp(currentAimAngle + delta, -paddleSettings.MaxAimAngle, paddleSettings.MaxAimAngle);
                    aimDirection.localRotation = Quaternion.AngleAxis(angleMultiplier * currentAimAngle, new Vector3(0, 0, 1));
                }
            }
        }

        private void FixedUpdate()
        {
            if (!aiming && moving)
            {
                float multiplier = Time.fixedDeltaTime * paddleSettings.PaddleSpeed;
                Vector2 delta = currentNormalizedVelocity;
                delta.x = 0;
                delta.y *= multiplier;

                Vector3 projectedPosition = paddleRigidbody2D.position + delta;
                Vector3 halfSize = paddleSpriteRenderer.transform.TransformVector(paddleSpriteRenderer.sprite.bounds.extents);

                float halfWidth = Mathf.Abs(halfSize.x);
                float halfHeight = Mathf.Abs(halfSize.y);

                projectedPosition.x = Mathf.Clamp(projectedPosition.x, gameBoundsMinWorldSpace.x + halfWidth, gameBoundsMaxWorldSpace.x - halfWidth);
                projectedPosition.y = Mathf.Clamp(projectedPosition.y, gameBoundsMinWorldSpace.y + halfHeight, gameBoundsMaxWorldSpace.y - halfHeight);

                paddleRigidbody2D.MovePosition(projectedPosition);
            }
        }

        #endregion

        private void ResetPaddle()
        {
            var pos = new Vector3(0, Screen.height * 0.5f, 0);
            var currentPosition = transform.position;
            currentPosition.y = Camera.main.ScreenToWorldPoint(pos).y;
            transform.position = currentPosition;
        }

        private IEnumerator RemoveStatusAfter(PaddleStatus status, float seconds)
        {
            yield return new WaitForSeconds(seconds);

            paddleState.RemoveStatus(status);
        }

        #region Callbacks

        public void OnMove(CallbackContext context)
        {
            moving = context.performed && !paddleState.HasStatus(PaddleStatus.Freeze);
            currentNormalizedVelocity = context.ReadValue<Vector2>();
        }

        public void OnAim(CallbackContext context)
        {
            aiming = context.performed;
        }

        public void OnPaddleStatusAdded(PaddleStatusAddedArgs statusAddedArgs)
        {
            PaddleStatus status = statusAddedArgs.statusApplied;

            if ((status & PaddleStatus.Freeze) == PaddleStatus.Freeze)
            {
                if (frozenEffectGameObject != null)
                {
                    frozenEffectGameObject.SetActive(true);
                }
            }

            if ((status & PaddleStatus.Enlarge) == PaddleStatus.Enlarge)
            {
                transform.localScale += new Vector3(paddleSettings.EnlargeScaleChange, 0, 0);
            }

            if ((status & PaddleStatus.Shrink) == PaddleStatus.Shrink)
            {
                transform.localScale -= new Vector3(paddleSettings.ShrinkScaleChange, 0, 0);
                Debug.Assert(transform.localScale.x >= (1 - paddleSettings.ShrinkScaleChange), $"Local Scale less than 0.5f.");
            }

            if ((status & PaddleStatus.Shield) == PaddleStatus.Shield)
            {
                if (shieldEffectGameObject != null)
                {
                    shieldEffectGameObject.SetActive(true);
                }
            }

            StartCoroutine(RemoveStatusAfter(status, statusAddedArgs.secondsAppliedFor));
        }

        public void OnPaddleStatusRemoved(PaddleStatusRemovedArgs statusRemovedArgs)
        {
            PaddleStatus status = statusRemovedArgs.statusRemoved;

            if ((status & PaddleStatus.Freeze) == PaddleStatus.Freeze)
            {
                if (frozenEffectGameObject != null)
                {
                    frozenEffectGameObject.SetActive(false);
                }
            }

            if ((status & PaddleStatus.Enlarge) == PaddleStatus.Enlarge)
            {
                transform.localScale -= new Vector3(paddleSettings.EnlargeScaleChange, 0, 0);
                Debug.Assert(transform.localScale.x >= (1 - paddleSettings.ShrinkScaleChange), $"Local Scale less than 0.5f.");
            }

            if ((status & PaddleStatus.Shrink) == PaddleStatus.Shrink)
            {
                transform.localScale += new Vector3(paddleSettings.ShrinkScaleChange, 0, 0);
            }

            if ((status & PaddleStatus.Shield) == PaddleStatus.Shield)
            {
                if (shieldEffectGameObject != null)
                {
                    shieldEffectGameObject.SetActive(false);
                }
            }
        }

        public void OnGameRoundReset()
        {
            ResetPaddle();

            paddleState.SetStatus(PaddleStatus.None);
        }

        #endregion
    }
}