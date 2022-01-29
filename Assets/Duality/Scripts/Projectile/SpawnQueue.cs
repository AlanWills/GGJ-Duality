using System.Collections.Generic;
using UnityEngine;

namespace Duality.Projectile
{
    [CreateAssetMenu(fileName = nameof(SpawnQueue), menuName = "Duality/Projectile/Spawn Queue")]
    public class SpawnQueue : ScriptableObject
    {
        #region Properties and Fields

        [SerializeField] private Celeste.Events.Event spawnQueueChanged;

        private Queue<ProjectileSettings> spawnQueue = new Queue<ProjectileSettings>();

        #endregion

        public void Hookup(List<ProjectileSettings> startingItems)
        {
            spawnQueue.Clear();

            for (int i = 0, n = startingItems.Count; i < n; ++i)
            {
                spawnQueue.Enqueue(startingItems[i]);
            }

            spawnQueueChanged.Invoke();
        }

        public ProjectileSettings Push(ProjectileSettings newItem)
        {
            ProjectileSettings spawnedItem = spawnQueue.Dequeue();
            spawnQueue.Enqueue(newItem);
            spawnQueueChanged.InvokeSilently();

            return spawnedItem;
        }
    }
}