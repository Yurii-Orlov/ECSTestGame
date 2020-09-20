using Zenject;
using UnityEngine;
using System.Collections.Generic;

namespace OrCor_GameName
{

    public class MenuState : GameStateEntity
    {
        private readonly GameStateManager _gameStateManager;
        private ScenesManager _scenesManager;
        private UIManager _uIManager;

        public MenuState(GameStateManager gameStateManager,
                         ScenesManager scenesManager,
                         UIManager uIManager)
        {
            _gameStateManager = gameStateManager;
            _scenesManager = scenesManager;
            _uIManager = uIManager;
        }

        public override void Initialize()
        {
            _scenesManager.OnSceneFinishedLoading += MenuState_OnSceneFinishedLoading;
        }

        public override void Start()
        {
            Debug.Log("Start menu state");
            _scenesManager.ChangeScene(SceneNamesConstants.SCENE_MENU_NAME, true);
            //_pages = new List<IUIElement>()
            //{
            //    new MenuPage(_gameClient)
            //};

            //_popups = new List<IUIPopup>()
            //{
            //    new ShopPopup(_gameClient),
            //    new SettingsPopup(_gameClient)
            //};
            //_uIManager.AddPages(_pages);
            //_uIManager.AddPopups(_popups);
        }

        public override void Tick()
        {
        }

        public override void Dispose()
        {
            _scenesManager.OnSceneFinishedLoading -= MenuState_OnSceneFinishedLoading;
            //_uIManager.RemovePages(_pages);
            //_uIManager.RemovePopups(_popups);
        }

        private void MenuState_OnSceneFinishedLoading()
        {
            _uIManager.SetPage<MenuPage>(true);
        }

        public class Factory : PlaceholderFactory<MenuState>
        {

        }

    }

}
