﻿using Celeste.Parameters;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Duality.GameRound
{
    public class GameRoundManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameRoundSettings gameRoundSettings;
        [SerializeField] private Celeste.Events.Event beginRound;
        [SerializeField] private Celeste.Events.Event endRound;

        [SerializeField] private GameObject countdownUI;
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private TextMeshProUGUI gameRoundTimeText;
        [SerializeField] private GameObject gameRoundOverUI;

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
            gameRoundTimeText.text = ToTimeString(Mathf.CeilToInt(gameRoundSettings.GameRoundSeconds));

            float currentCountdown = gameRoundSettings.CountdownSeconds;
            float currentSecond = 1;

            countdownUI.SetActive(true);
            gameRoundOverUI.SetActive(false);

            while (currentCountdown > 0)
            {
                // Cubic lerp
                countdownText.text = Mathf.CeilToInt(currentCountdown).ToString();
                countdownText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, currentSecond * currentSecond * currentSecond);

                yield return null;

                currentCountdown -= Time.deltaTime;
                currentSecond -= Time.deltaTime;

                if (currentSecond <= 0)
                {
                    currentSecond = 1;
                }
            }

            currentSecond = 1;
            countdownText.text = "GO!";

            while (currentSecond > 0)
            {
                countdownText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, currentSecond * currentSecond * currentSecond);

                yield return null;

                currentSecond -= Time.deltaTime;
            }

            countdownUI.SetActive(false);
            beginRound.Invoke();

            float gameRoundTimeRemaining = gameRoundSettings.GameRoundSeconds;
            while (gameRoundTimeRemaining > 0)
            {
                yield return null;

                gameRoundTimeText.text = ToTimeString(Mathf.CeilToInt(gameRoundTimeRemaining));
                gameRoundTimeRemaining -= Time.deltaTime;
            }

            gameRoundTimeText.text = "0:00";
            endRound.Invoke();

            currentSecond = 1;
            countdownText.text = "Time's Up!";

            while (currentSecond > 0)
            {
                countdownText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, currentSecond * currentSecond * currentSecond);

                yield return null;

                currentSecond -= Time.deltaTime;
            }

            yield return new WaitForSeconds(1);

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