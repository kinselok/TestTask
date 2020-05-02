using System;
using UnityEngine;

namespace SlotInput
{
    public class BaseInput : MonoBehaviour
    {
        public static event Action Play;

        protected virtual void OnPlay()
        {
            Play?.Invoke();
        }

        public event Action<int> BetChanged;

        protected virtual void OnBetChanged(int amount)
        {
            BetChanged?.Invoke(amount);
        }
    }
}
