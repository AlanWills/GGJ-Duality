using Celeste.DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality.Projectile
{
    [CreateAssetMenu(fileName = nameof(ProjectileSpawnerSettings), menuName = "Duality/Projectile/Projectile Spawner Settings")]
    public class ProjectileSpawnerSettings : ScriptableObject
    {
        #region Properties and Fields

        public float InitialSpawnDelay => initialSpawnDelay;
        public float SecondsBetweenSpawns => secondsBetweenSpawns;
        public float SpawnVelocity => spawnVelocity;

        [SerializeField] private float initialSpawnDelay = 5;
        [SerializeField] private float secondsBetweenSpawns = 12;
        [SerializeField] private int spawnQueueSize = 4;
        [SerializeField] private float spawnVelocity = 3;
        [SerializeField] private ProjectileDistribution[] projectiles;
        
        private ShuffleBag<ProjectileSettings> spawnShuffleBag;

        #endregion

        public void Hookup()
        {
            int total = 0;

            for (int i = 0, n = projectiles.Length; i < n; ++i)
            {
                total += projectiles[i].shuffleBagSize;
            }

            spawnShuffleBag = new ShuffleBag<ProjectileSettings>(total);

            for (int i = 0, n = projectiles.Length; i < n; ++i)
            {
                spawnShuffleBag.Add(projectiles[i].projectileSettings, projectiles[i].shuffleBagSize);
            }
        }

        public List<ProjectileSettings> CreateStartingSpawnQueue()
        {
            List<ProjectileSettings> startingSpawnQueue = new List<ProjectileSettings>();

            for (int i = 0; i < spawnQueueSize; ++i)
            {
                startingSpawnQueue.Add(GetNextItem());
            }

            return startingSpawnQueue;
        }

        public ProjectileSettings GetNextItem()
        {
            return spawnShuffleBag.Next();
        }
    }
}