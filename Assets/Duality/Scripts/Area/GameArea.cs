using Celeste.UI.Layout;
using Celeste.Utils;
using System.Collections;
using UnityEngine;

namespace Duality.Area
{
    public class GameArea : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private ReferenceLayout gameAreaReference;
        [SerializeField] private BoxCollider2D gameAreaLeft;
        [SerializeField] private BoxCollider2D gameAreaRight;
        [SerializeField] private BoxCollider2D gameAreaTop;
        [SerializeField] private BoxCollider2D gameAreaBottom;

        #endregion

        private void Start()
        {
            Rect gameBoundsUISpace = gameAreaReference.rectTransform.GetWorldRect();
            Vector3 gameBoundsMinWorldSpace = Camera.main.ScreenToWorldPoint(new Vector3(gameBoundsUISpace.xMin, gameBoundsUISpace.yMin, 0));
            Vector3 gameBoundsMaxWorldSpace = Camera.main.ScreenToWorldPoint(new Vector3(gameBoundsUISpace.xMax, gameBoundsUISpace.yMax, 0));

            float gameAreaWidth = gameBoundsMaxWorldSpace.x - gameBoundsMinWorldSpace.x;
            float gameAreaHeight = gameBoundsMaxWorldSpace.y - gameBoundsMinWorldSpace.y;

            // Game area left
            {
                gameAreaLeft.offset = new Vector2(gameBoundsMinWorldSpace.x - 50, gameBoundsMinWorldSpace.y + gameAreaHeight * 0.5f);
                gameAreaLeft.size = new Vector2(100, gameAreaHeight);
            }

            // Game area right
            {
                gameAreaRight.offset = new Vector2(gameBoundsMaxWorldSpace.x + 50, gameBoundsMinWorldSpace.y + gameAreaHeight * 0.5f);
                gameAreaRight.size = new Vector2(100, gameAreaHeight);
            }

            // Game area top
            {
                gameAreaTop.offset = new Vector2(gameBoundsMinWorldSpace.x + gameAreaWidth * 0.5f, gameBoundsMaxWorldSpace.y + 50);
                gameAreaTop.size = new Vector2(gameAreaWidth, 100);
            }

            // Game area bottom
            {
                gameAreaBottom.offset = new Vector2(gameBoundsMinWorldSpace.x + gameAreaWidth * 0.5f, gameBoundsMinWorldSpace.y - 50);
                gameAreaBottom.size = new Vector2(gameAreaWidth, 100);
            }
        }
    }
}