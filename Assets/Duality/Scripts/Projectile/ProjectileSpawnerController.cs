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
        [SerializeField] private IntReference playerMask;
        [SerializeField] private IntReference opponentMask;
        [SerializeField] private ProjectileAllocator projectileAllocator;
        [SerializeField] private Transform projectileSpawnAnchor;

        private bool spawning = false;
        private Coroutine spawnCoroutine;

        #endregion

        #region Unity Methods

        private void Start()
        {
            HookupSpawning();
        }

        #endregion

        #region Spawning

        private void HookupSpawning()
        {
            projectileSpawnerSettings.Hookup();
            spawnQueue.Hookup(projectileSpawnerSettings.CreateStartingSpawnQueue());
        }

        private void StartSpawning()
        {
            spawning = true;
            spawnCoroutine = StartCoroutine(SpawnCoroutine());
        }

        private void StopSpawning()
        {
            spawning = false;

            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
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
                    playerMask.Value,
                    opponentMask.Value);
                
                yield return new WaitForSeconds(projectileSpawnerSettings.SecondsBetweenSpawns);
            }
        }

        #endregion

        #region Callbacks

        public void OnGameRoundBegun()
        {
            StartSpawning();
        }

        public void OnGameRoundEnd()
        {
            StopSpawning();
        }

        public void OnGameRoundReset()
        {
            HookupSpawning();
        }

        #endregion
    }
}