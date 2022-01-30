using Duality.GameRound;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace DualityEditor.GameRound
{
    [CustomEditor(typeof(GameRoundManager))]
    public class GameRoundManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GameRoundManager gameRoundManager = target as GameRoundManager;

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("-60s"))
                {
                    gameRoundManager.ModifyTime(-60);
                }

                if (GUILayout.Button("-30s"))
                {
                    gameRoundManager.ModifyTime(-30);
                }

                if (GUILayout.Button("-10s"))
                {
                    gameRoundManager.ModifyTime(-10);
                }
            }

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("+60s"))
                {
                    gameRoundManager.ModifyTime(60);
                }

                if (GUILayout.Button("+30s"))
                {
                    gameRoundManager.ModifyTime(30);
                }

                if (GUILayout.Button("+10s"))
                {
                    gameRoundManager.ModifyTime(10);
                }
            }

            base.OnInspectorGUI();
        }
    }
}