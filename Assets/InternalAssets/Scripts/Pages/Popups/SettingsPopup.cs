using System;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace OrCor_GameName
{

    public class SettingsPopup : IInitializable, IDisposable, IUIPopup
    {
        public event Action OnPopupWasInited = delegate { };

        public GameObject SelfPage { get; set; }

        private readonly UIManager _uIManager;
        private readonly LoadObjectsManager _loadObjectsManager;
        private readonly GameStateManager _gameStateManager;

        private SettingsPopupComponent _pageComponent;

        private UniTask<UnityEngine.Object>.Awaiter _awaiter;

        public SettingsPopup(UIManager uIManager,
                             LoadObjectsManager loadObjectsManager,
                             GameStateManager gameStateManager)
        {
            _uIManager = uIManager;
            _loadObjectsManager = loadObjectsManager;
            _gameStateManager = gameStateManager;
            _uIManager.AddPopup(this);

            _awaiter = _loadObjectsManager.GetObjectByPath(Constants.PATH_TO_UI_PREFABS + "Popups/SettingsPopup").GetAwaiter();

            _awaiter.OnCompleted(() => {
                SelfPage = MonoBehaviour.Instantiate(_awaiter.GetResult() as GameObject);
                SelfPage.transform.SetParent(_uIManager.Canvas.transform, false);
                _pageComponent = SelfPage.GetComponent<SettingsPopupComponent>();
                OnPopupWasInited();
            });


        }

        public void Initialize()
        {
            _awaiter.OnCompleted(() =>
            {
                _pageComponent.Info.menuBtn.onClick.AddListener(MenuBtnClick);

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

        public void Show(object data)
        {
            Show();
        }

        public void Dispose()
        {
            _uIManager.RemovePopup(this);
            MonoBehaviour.Destroy(SelfPage);
            //this.Dispose();
        }

        private void MenuBtnClick()
        {
            _uIManager.SetPage<MenuPage>(true);
            Hide();
        }

        public void SetMainPriority()
        {
            SelfPage.transform.SetAsLastSibling();
        }

    }

}

