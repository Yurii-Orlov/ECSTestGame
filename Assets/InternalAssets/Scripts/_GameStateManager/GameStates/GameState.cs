using Zenject;
using UnityEngine;
using System.Collections.Generic;

namespace OrCor_GameName
{
    public class GameState : GameStateEntity
    {
        private readonly GameStateManager _gameStateManager;
        private ScenesManager _scenesManager;
        private UIManager _uIManager;

        public GameState(GameStateManager gameStateManager,
                         ScenesManager scenesManager,
                         UIManager uIManager)
        {
            _gameStateManager = gameStateManager;
            _scenesManager = scenesManager;
            _uIManager = uIManager;
        }

        public override void Initialize()
        {
            _scenesManager.OnSceneFinishedLoading += GameplayState_OnSceneFinishedLoading;
        }

        public override void Start()
        {
            _scenesManager.ChangeScene(SceneNamesConstants.SCENE_GAME_NAME, true);

            //_pages = new List<IUIElement>()
            //{
            //    new GamePage(_gameClient)
            //};
            Debug.Log("Gameplay state started");
        }

        public override void Tick()
        {

        }

        public override void Dispose()
        {
            _scenesManager.OnSceneFinishedLoading -= GameplayState_OnSceneFinishedLoading;
        }

        private void GameplayState_OnSceneFinishedLoading()
        {
            _uIManager.SetPage<GamePage>(true);
            _uIManager.HideAllPopups();
        }

        public class Factory : PlaceholderFactory<GameState>
        {

        }

    }

}


