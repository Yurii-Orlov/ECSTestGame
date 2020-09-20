using System;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace OrCor_GameName
{

    public class GamePage : IInitializable, IDisposable, IUIElement
    {
        public GameObject SelfPage { get; set; }
        public GamePageComponent PageComponent { get; private set; }

        private UIManager _uiManager;
        private LoadObjectsManager _loadObjectsManager;
        private ScenesManager _scenesManager;
        private GameStateManager _gameStateManager;

        private UniTask<UnityEngine.Object>.Awaiter _awaiter;

        public GamePage(UIManager uiManager,
                        LoadObjectsManager loadObjectsManager,
                        ScenesManager scenesManager,
                        GameStateManager gameStateManager)
        {
            _uiManager = uiManager;
            _loadObjectsManager = loadObjectsManager;
            _scenesManager = scenesManager;
            _gameStateManager = gameStateManager;

            _uiManager.AddPage(this);
            _awaiter = _loadObjectsManager.GetObjectByPath(Constants.PATH_TO_UI_PREFABS + "GamePage").GetAwaiter();

            _awaiter.OnCompleted(() => {
                SelfPage = MonoBehaviour.Instantiate(_awaiter.GetResult() as GameObject);
                SelfPage.transform.SetParent(_uiManager.Canvas.transform, false);
                PageComponent = SelfPage.GetComponent<GamePageComponent>();
            });

        }

        public void Initialize()
        {
            _awaiter.OnCompleted(() => {
                PageComponent.Info.menuBtn.onClick.AddListener(MenuBtnClick);

                Hide();
            });


        }

        public void Hide()
        {
            SelfPage.SetActive(false);
        }

        public void Show()
        {
            SelfPage.SetActive(true);
        }

        public void Dispose()
        {
            _uiManager.RemovePage(this);
            MonoBehaviour.Destroy(SelfPage);
        }

        private void MenuBtnClick()
        {
            _gameStateManager.ChangeState(Enumerators.GameStateTypes.MENU);
        }

    }

}

