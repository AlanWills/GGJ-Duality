using Celeste.DataStructures;
using Celeste.Events;
using Celeste.Parameters;
using System;
using System.Collections;
using UnityEngine;

namespace Duality.Projectile
{
    [CreateAssetMenu(fileName = nameof(ProjectileCommonEvents), menuName = "Duality/Projectile/Projectile Common Events")]
    public class ProjectileCommonEvents : ScriptableObject
    {
        [Serializable]
        public struct AddPointsForPlayer
        {
            public IntValue playerMask;
            public IntEvent addPoints;
        }

        #region Properties and Fields

        public AddPointsForPlayer[] addPointsForPlayerEvents;

        #endregion

        public void AddPoints(int playerMask, int points)
        {
            var addPointsForPlayer = addPointsForPlayerEvents.Find(x => x.playerMask.Value == playerMask);
            Debug.Assert(addPointsForPlayer.addPoints != null, $"No add points for player event set for Player {playerMask}.");

            if (addPointsForPlayer.addPoints != null)
            {
                addPointsForPlayer.addPoints.Invoke(points);
            }
        }
    }
}