using Celeste.Memory;
using System.Collections;
using UnityEngine;

namespace Duality.FX
{
    public class ExplosionManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObjectAllocator explosionAllocator;

        #endregion

        #region Callbacks

        public void OnSpawnExplosion(Vector3 position)
        {
            GameObject gameObject = explosionAllocator.AllocateWithResizeIfNecessary();
            gameObject.transform.position = position;
            gameObject.SetActive(true);
        }

        #endregion
    }
}