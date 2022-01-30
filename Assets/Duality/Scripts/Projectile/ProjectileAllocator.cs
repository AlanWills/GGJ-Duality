using Celeste.DataStructures;
using Celeste.Memory;
using System;
using System.Collections;
using UnityEngine;

namespace Duality.Projectile
{
    public class ProjectileAllocator : MonoBehaviour
    {
        [Serializable]
        private struct AllocatorForProjectile
        {
            public ProjectileSettings projectileSettings;
            public GameObjectAllocator allocator;
        }

        #region Properties and Fields

        [SerializeField] private AllocatorForProjectile[] allocators;

        #endregion

        public GameObject Allocate(ProjectileSettings projectileSettings)
        {
            AllocatorForProjectile allocator = allocators.Find(x => x.projectileSettings == projectileSettings);
            if (allocator.allocator != null)
            {
                return allocator.allocator.AllocateWithResizeIfNecessary();
            }

            return null;
        }

        #region Callbacks

        public void OnGameRoundEnd()
        {
            for (int i = 0, n = allocators.Length; i < n; ++i)
            {
                allocators[i].allocator.DeallocateAll();
            }
        }

        #endregion
    }
}