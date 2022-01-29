using Celeste.Events;
using Duality.Player;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Duality.Events
{
    [Serializable]
    public struct PaddleStatusAddedArgs
    {
        public PaddleStatus statusApplied;
        public float secondsAppliedFor;
    }

    [Serializable]
    public class PaddleStatusAddedUnityEvent : UnityEvent<PaddleStatusAddedArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(PaddleStatusAddedEvent), menuName = "Duality/Events/Paddle/Paddle Status Added Event")]
    public class PaddleStatusAddedEvent : ParameterisedEvent<PaddleStatusAddedArgs>
    {
    }
}
