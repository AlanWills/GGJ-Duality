using Duality.Projectile;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Duality.UI
{
    public class ProjectileUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Image projectileUI;

        #endregion

        public void Hookup(ProjectileSettings projectileSettings)
        {
            projectileUI.sprite = projectileSettings.UISprite;
        }
    }
}