using Celeste.Parameters;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duality.GameRound
{
    public class GameRoundManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private IntValue player1Score;
        [SerializeField] private IntValue player2Score;
        [SerializeField] private GameRoundSettings gameRoundSettings;
        [SerializeField] private IntValue currentGameRoundTimeRemaining;
        [SerializeField] private Celeste.Events.Event beginRound;
        [SerializeField] private Celeste.Events.Event endRound;

        [SerializeField] private GameObject countdownUI;
        [SerializeField] private Image countdownBackground;
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private TextMeshProUGUI gameRoundTimeText;
        [SerializeField] private GameObject gameRoundOverUI;

        [SerializeField] private Color normalTimeColour = Color.green;
        [SerializeField] private Color timeAlmostUpColour = Color.red;

        [SerializeField] private UnityEvent countdownTick;
        [SerializeField] private UnityEvent countdownDone;
        [SerializeField] private UnityEvent timeAlmostUpTick;

        private Coroutine gameRoundCoroutine;

        #endregion

        #region Unity Methods

        private void Start()
        {
            StartGameRound();
        }

        #endregion

        #region Callbacks

        public void OnGameRoundReset()
        {
            StartGameRound();
        }

        #endregion

        public void ModifyTime(int seconds)
        {
            currentGameRoundTimeRemaining += seconds;
        }

        private void StartGameRound()
        {
            if (gameRoundCoroutine != null)
            {
                StopCoroutine(gameRoundCoroutine);
                gameRoundCoroutine = null;
            }

            gameRoundCoroutine = StartCoroutine(GameRound());
        }

        private IEnumerator GameRound()
        {
            player1Score.Value = 0;
            player2Score.Value = 0;

            float currentCountdown = gameRoundSettings.CountdownSeconds;
            float currentSecond = 1;
            countdownTick.Invoke();

            gameRoundTimeText.text = ToTimeString(Mathf.CeilToInt(gameRoundSettings.GameRoundSeconds));
            currentGameRoundTimeRemaining.Value = gameRoundSettings.GameRoundSeconds;
            countdownBackground.color = normalTimeColour;

            countdownUI.SetActive(true);
            gameRoundOverUI.SetActive(false);

            while (currentCountdown > 0)
            {
                if (currentSecond <= 0)
                {
                    currentSecond = 1;
                    countdownTick.Invoke();
                }

                // Cubic lerp
                countdownText.text = Mathf.CeilToInt(currentCountdown).ToString();
                countdownText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, currentSecond * currentSecond * currentSecond);

                yield return null;

                currentCountdown -= Time.deltaTime;
                currentSecond -= Time.deltaTime;
            }

            currentSecond = 1;
            countdownText.text = "GO!";
            countdownDone.Invoke();

            while (currentSecond > 0)
            {
                countdownText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, currentSecond * currentSecond * currentSecond);

                yield return null;

                currentSecond -= Time.deltaTime;
            }

            countdownUI.SetActive(false);
            beginRound.Invoke();

            while (currentGameRoundTimeRemaining.Value > 0)
            {
                gameRoundTimeText.text = ToTimeString(currentGameRoundTimeRemaining.Value);

                yield return new WaitForSeconds(1);

                currentGameRoundTimeRemaining -= 1;

                if (currentGameRoundTimeRemaining.Value <= gameRoundSettings.TimeAlmostUp)
                {
                    countdownBackground.color = timeAlmostUpColour;
                    timeAlmostUpTick.Invoke();
                }
            }

            gameRoundTimeText.text = "0:00";
            endRound.Invoke();

            currentSecond = 1;
            countdownUI.SetActive(true);
            countdownText.text = "Time's Up!";

            while (currentSecond > 0)
            {
                countdownText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, currentSecond * currentSecond * currentSecond);

                yield return null;

                currentSecond -= Time.deltaTime;
            }

            yield return new WaitForSeconds(1);

            countdownUI.SetActive(false);
            gameRoundOverUI.SetActive(true);
        }

        private string ToTimeString(int seconds)
        {
            int minutes = seconds / 60;
            int remainingSeconds = seconds - minutes * 60;

            if (remainingSeconds < 10)
            {
                return $"{minutes}:0{remainingSeconds}";
            }
            else
            {
                return $"{minutes}:{remainingSeconds}";
            }
        }
    }
}