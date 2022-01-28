using Celeste.Parameters;
using Celeste.Tools.Attributes.GUI;
using System.Collections;
using UnityEngine;

namespace Duality.Player
{
    [CreateAssetMenu(fileName = nameof(PaddleSettings), menuName = "Duality/Player/Paddle Settings")]
    public class PaddleSettings : ScriptableObject
    {
        #region Properties and Fields

        public bool OverridePaddleColour => overridePaddleColour;
        public Color PaddleColour => paddleColour;

        public float PaddleSpeed => paddleSpeed.Value;

        [Header("Visuals")]
        [SerializeField] private bool overridePaddleColour = true;
        [SerializeField, ShowIf(nameof(overridePaddleColour))] private Color paddleColour = Color.white;

        [Header("Input")]
        [SerializeField] private FloatValue paddleSpeed;

        #endregion
    }
}