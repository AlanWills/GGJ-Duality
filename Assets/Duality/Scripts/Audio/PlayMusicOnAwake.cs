using Celeste.Events;
using System.Collections;
using UnityEngine;

namespace Duality.Audio
{
    public class PlayMusicOnAwake : MonoBehaviour
    {
        [SerializeField] private AudioClipEvent playMusic;
        [SerializeField] private AudioClip music;

        private void Awake()
        {
            playMusic.Invoke(music);
        }
    }
}