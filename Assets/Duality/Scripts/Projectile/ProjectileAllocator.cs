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

        [SerializeField] private AllocatorForProjectile[] allocators;

        public GameObject Allocate(ProjectileSettings projectileSettings)
        {
            AllocatorForProjectile allocator = allocators.Find(x => x.projectileSettings == projectileSettings);
            if (allocator.allocator != null)
            {
                return allocator.allocator.AllocateWithResizeIfNecessary();
            }

            return null;
        }
    }
}