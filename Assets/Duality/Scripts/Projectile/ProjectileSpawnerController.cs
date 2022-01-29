using Celeste.Maths;
using Celeste.Parameters;
using System.Collections;
using UnityEngine;

namespace Duality.Projectile
{
    public class ProjectileSpawnerController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private ProjectileSpawnerSettings projectileSpawnerSettings;
        [SerializeField] private SpawnQueue spawnQueue;
        [SerializeField] private IntValue playerMask;
        [SerializeField] private ProjectileAllocator projectileAllocator;
        [SerializeField] private Transform projectileSpawnAnchor;

        private bool spawning;

        #endregion

        #region Unity Methods

        private void Start()
        {
            projectileSpawnerSettings.Hookup();
            spawnQueue.Hookup(projectileSpawnerSettings.CreateStartingSpawnQueue());

            StartSpawning();
        }

        #endregion

        #region Spawning

        private void StartSpawning()
        {
            spawning = true;

            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            yield return new WaitForSeconds(projectileSpawnerSettings.InitialSpawnDelay);

            while (spawning)
            {
                ProjectileSettings projectileSettings = spawnQueue.Push(projectileSpawnerSettings.GetNextItem());
                GameObject projectileGameObject = projectileAllocator.Allocate(projectileSettings);
                ProjectileController projectileController = projectileGameObject.GetComponent<ProjectileController>();
                projectileController.Hookup(
                    projectileSpawnAnchor.position,
                    projectileSpawnAnchor.rotation,
                    projectileSpawnerSettings.SpawnVelocity,
                    playerMask.Value);
                
                yield return new WaitForSeconds(projectileSpawnerSettings.SecondsBetweenSpawns);
            }
        }

        #endregion
    }
}