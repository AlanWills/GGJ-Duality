using Celeste.Scene;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duality
{
    public class LoadSceneSet : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private bool loadOnAwake;
        [SerializeField] private SceneSet sceneSet;

        #endregion

        private void Awake()
        {
            if (loadOnAwake)
            {
                Load();
            }
        }

        public void Load()
        {
            StartCoroutine(sceneSet.LoadAsync(LoadSceneMode.Single, (f) => { }, () => { }));
        }
    }
}