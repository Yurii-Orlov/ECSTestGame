using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OrCor_GameName
{
    /// <summary>
    /// Scriptable object used for game states
    /// </summary>
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    public class GeneralGameSettingsInstaller : ScriptableObjectInstaller<GeneralGameSettingsInstaller>
    {
        public GameStateSettings gameState;

        [Serializable]
        public class GameStateSettings
        {
            //public GameOverState.Settings gameOverState;
            //public GameVictoryState.Settings victoryState;
        }

        public override void InstallBindings()
        {
            //Container.BindInstance(gameState.gameOverState);
            //Container.BindInstance(gameState.victoryState);  
        }
    }
}
