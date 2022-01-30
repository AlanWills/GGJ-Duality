using Duality.Events;
using System;
using System.Collections;
using UnityEngine;

namespace Duality.Player
{
    [Serializable, Flags]
    public enum PaddleStatus
    { 
        None = 0,
        Shield = 1 << 1,
        Freeze = 1 << 2,
        Enlarge = 1 << 3,
        Shrink = 1 << 4
    }

    [CreateAssetMenu(fileName = nameof(PaddleState), menuName = "Duality/Player/Paddle State")]
    public class PaddleState : ScriptableObject
    {
        #region Properties and Fields

        [SerializeField] private PaddleStatusAddedEvent onStatusAdded;
        [SerializeField] private PaddleStatusRemovedEvent onStatusRemoved;
        
        [NonSerialized] private PaddleStatus status = PaddleStatus.None;

        #endregion

        public bool HasStatus(PaddleStatus status)
        {
            return (this.status & status) != PaddleStatus.None;
        }

        public void AddStatus(PaddleStatus status, float applyForSeconds)
        {
            if (!HasStatus(status))
            {
                this.status |= status;
                onStatusAdded.Invoke(new PaddleStatusAddedArgs() { statusApplied = status, secondsAppliedFor = applyForSeconds });
            }
        }

        public void RemoveStatus(PaddleStatus status)
        {
            if (HasStatus(status))
            {
                this.status &= ~status;
                onStatusRemoved.Invoke(new PaddleStatusRemovedArgs() { statusRemoved = status });
            }
        }

        public void SetStatus(PaddleStatus status)
        {
            if (this.status != status)
            {
                PaddleStatus statusToRemove = this.status & ~status;
                RemoveStatus(statusToRemove);
            }
        }
    }
}