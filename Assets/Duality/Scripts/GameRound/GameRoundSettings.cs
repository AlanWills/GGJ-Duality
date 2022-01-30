using System.Collections;
using UnityEngine;

namespace Duality.GameRound
{
    [CreateAssetMenu(fileName = nameof(GameRoundSettings), menuName = "Duality/Game Round Settings")]
    public class GameRoundSettings : ScriptableObject
    {
        public int CountdownSeconds => countdownSeconds;
        public int GameRoundSeconds => gameRoundSeconds;
        public int TimeAlmostUp => timeAlmostUp;

        [SerializeField] private int countdownSeconds = 3;
        [SerializeField] private int gameRoundSeconds = 150;
        [SerializeField] private int timeAlmostUp = 10;
    }
}