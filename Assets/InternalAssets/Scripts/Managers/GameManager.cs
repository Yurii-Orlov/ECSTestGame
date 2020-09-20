using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OrCor_GameName
{

    public class GameManager : IInitializable, IDisposable
    {
        public bool IsGameRuning { get ; set ; }
        public Action OnRestartGame { get; set; }
        public Action OnStartGame { get ; set ; }

        [Inject]
        private TestSettings _testSettings;

        public GameManager()
        {

        }

        public void Initialize()
        {
            Debug.Log("Start game");
            Debug.Log("_testSettings = " + _testSettings.testNum);
        }


        public void PauseGame()
        {
            
        }

        public void RestartGame()
        {
            OnRestartGame?.Invoke();
        }

        public void StartGame()
        {
            OnStartGame?.Invoke();
        }

        public void StopGame()
        {
            IsGameRuning = false;
        }

        public void Dispose()
        {

        }


    }
}
