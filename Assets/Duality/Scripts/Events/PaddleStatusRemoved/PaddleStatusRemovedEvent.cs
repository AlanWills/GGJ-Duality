using Celeste.Events;
using Duality.Player;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Duality.Events
{
    [Serializable]
    public struct PaddleStatusRemovedArgs
    {
        public PaddleStatus statusRemoved;
    }

    [Serializable]
    public class PaddleStatusRemovedUnityEvent : UnityEvent<PaddleStatusRemovedArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(PaddleStatusRemovedEvent), menuName = "Duality/Events/Paddle/Paddle Status Removed Event")]
    public class PaddleStatusRemovedEvent : ParameterisedEvent<PaddleStatusRemovedArgs>
    {
    }
}
