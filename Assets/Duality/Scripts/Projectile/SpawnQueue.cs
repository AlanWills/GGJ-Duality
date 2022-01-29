using Celeste.DataStructures;
using System.Collections.Generic;
using UnityEngine;

namespace Duality.Projectile
{
    [CreateAssetMenu(fileName = nameof(SpawnQueue), menuName = "Duality/Projectile/Spawn Queue")]
    public class SpawnQueue : ScriptableObject
    {
        #region Properties and Fields

        public int NumItemsInQueue => spawnQueue.Count;

        [SerializeField] private Celeste.Events.Event spawnQueueChanged;

        private List<ProjectileSettings> spawnQueue = new List<ProjectileSettings>();

        #endregion

        public void Hookup(List<ProjectileSettings> startingItems)
        {
            spawnQueue.Clear();

            for (int i = 0, n = startingItems.Count; i < n; ++i)
            {
                spawnQueue.Add(startingItems[i]);
            }

            spawnQueueChanged.Invoke();
        }

        public ProjectileSettings Push(ProjectileSettings newItem)
        {
            ProjectileSettings spawnedItem = spawnQueue[0];
            spawnQueue.RemoveAt(0);
            spawnQueue.Add(newItem);
            spawnQueueChanged.InvokeSilently();

            return spawnedItem;
        }

        public ProjectileSettings GetItem(int index)
        {
            return spawnQueue.Get(index);
        }
    }
}