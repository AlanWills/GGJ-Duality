using Celeste.Tools;
using Duality.Player;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Duality.Input
{
    public class PaddleController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private PaddleSettings paddleSettings;
        [SerializeField] private Rigidbody2D paddleRigidbody2D;
        [SerializeField] private SpriteRenderer paddleSpriteRenderer;

        private bool moving;
        private Vector2 currentNormalizedVelocity;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref paddleRigidbody2D);
            this.TryGet(ref paddleSpriteRenderer);
        }

        private void Awake()
        {
            if (paddleSettings.OverridePaddleColour)
            {
                paddleSpriteRenderer.color = paddleSettings.PaddleColour;
            }
        }

        private void Start()
        {
            var pos = new Vector3(0, Screen.height * 0.5f, 0);
            var currentPosition = transform.position;
            currentPosition.y = Camera.main.ScreenToWorldPoint(pos).y;
            transform.position = currentPosition;
        }

        private void FixedUpdate()
        {
            if (moving)
            {
                float multiplier = Time.fixedDeltaTime * paddleSettings.PaddleSpeed;
                Vector2 amount = currentNormalizedVelocity;
                amount.x = 0;
                amount.y *= multiplier;

                paddleRigidbody2D.MovePosition(paddleRigidbody2D.position + amount);
            }
        }

        #endregion

        #region Callbacks

        public void OnMove(CallbackContext context)
        {
            moving = context.performed;
            currentNormalizedVelocity = context.ReadValue<Vector2>();
        }

        #endregion
    }
}