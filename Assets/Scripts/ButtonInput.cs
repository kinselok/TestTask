using System;
using UnityEngine;
using UnityEngine.UI;

namespace SlotInput
{
    public class ButtonInput :  BaseInput
    {
        #region Editor
        [SerializeField]
        private Button playButton;
        [SerializeField]
        private Button lowerBet;
        [SerializeField]
        private Button raiseBet;
        #endregion


        void Awake()
        {
            playButton.onClick.AddListener( () =>
            {
                if(SlotMachine.isInGame)
                    return;

                SlotMachine.isInGame = true;
                OnPlay();
            });
            lowerBet.onClick.AddListener( () => OnBetChanged(-1) );
            raiseBet.onClick.AddListener( () => OnBetChanged(1) );
        }
    }
}
