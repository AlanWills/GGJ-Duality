using Celeste.Memory;
using Duality.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality.UI
{
    public class SpawnQueueUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private SpawnQueue spawnQueue;
        [SerializeField] private GameObjectAllocator projectileUIAllocator;

        private List<ProjectileUIController> projectileUIControllers = new List<ProjectileUIController>();

        #endregion

        private void RebuildUI()
        {
            if (projectileUIControllers.Count != spawnQueue.NumItemsInQueue)
            {
                // Increase number of active controllers if necessary
                for (int i = 0, nToAllocate = Mathf.Max(0, spawnQueue.NumItemsInQueue - projectileUIControllers.Count); i < nToAllocate; ++i)
                {
                    GameObject projectileUI = projectileUIAllocator.AllocateWithResizeIfNecessary();
                    ProjectileUIController projectileUIController = projectileUI.GetComponent<ProjectileUIController>();
                    Debug.Assert(projectileUIController != null, $"No {nameof(ProjectileUIController)} found on {projectileUI.name}.");
                    projectileUIControllers.Add(projectileUIController);
                    projectileUI.SetActive(true);
                }

                // Decrease number of active controllers if necessary
                for (int i = 0, nToDeallocate = Mathf.Max(0, projectileUIControllers.Count - spawnQueue.NumItemsInQueue); i < nToDeallocate; ++i)
                {
                    int listIndex = projectileUIControllers.Count - i - 1;
                    projectileUIControllers[listIndex].gameObject.SetActive(false);
                    projectileUIControllers.RemoveAt(listIndex);
                }
            }

            Debug.Assert(projectileUIControllers.Count == spawnQueue.NumItemsInQueue);

            // Rebind the active controllers
            for (int i = 0, n = spawnQueue.NumItemsInQueue; i < n; ++i)
            {
                ProjectileSettings spawnQueueItem = spawnQueue.GetItem(i);
                Debug.Assert(spawnQueueItem != null, $"Spawn Queue item at {i} is null.");
                projectileUIControllers[i].Hookup(spawnQueueItem);
            }
        }

        #region Callbacks

        public void OnSpawnQueueChanged()
        {
            RebuildUI();
        }

        #endregion
    }
}