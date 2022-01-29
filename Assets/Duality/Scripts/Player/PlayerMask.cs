using Celeste.Parameters;
using System.Collections;
using UnityEngine;

namespace Duality.Player
{
    public class PlayerMask : MonoBehaviour
    {
        public int Mask => playerMask.Value;

        [SerializeField] private IntValue playerMask;
    }
}