using Celeste.Parameters;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Duality.GameRound
{
    public class GameRoundResultsUIController : MonoBehaviour
    {
        #region Properties and Fields

        [Header("Data")]
        [SerializeField] private IntValue player1Score;
        [SerializeField] private IntValue player2Score;
        [SerializeField] private StringValue player1Name;
        [SerializeField] private StringValue player2Name;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI roundResults;

        #endregion

        private void OnEnable()
        {
            if (player1Score.Value > player2Score.Value)
            {
                roundResults.text = $"{player1Name.Value} won!";
            }
            else if (player1Score.Value < player2Score.Value)
            {
                roundResults.text = $"{player2Name.Value} won!";
            }
            else
            {
                roundResults.text = "It's a tie!\n\nThat calls for a re-match...";
            }
        }
    }
}