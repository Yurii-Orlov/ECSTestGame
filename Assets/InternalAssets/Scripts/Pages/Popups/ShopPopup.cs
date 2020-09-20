using System;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace OrCor_GameName {

    public class ShopPopup : IInitializable, IDisposable, IUIPopup
    {

        public event Action OnPopupWasInited = delegate { };

        public GameObject SelfPage { get; set; }
        public ShopPopupComponent PageComponent { get; private set; }

        private readonly UIManager _uIManager;
        private readonly LoadObjectsManager _loadObjectsManager;
        private readonly GameStateManager _gameStateManager;

        private UniTask<UnityEngine.Object>.Awaiter _awaiter;

        public ShopPopup(UIManager uIManager, LoadObjectsManager loadObjectsManager, GameStateManager gameStateManager)
        {
            _uIManager = uIManager;
            _loadObjectsManager = loadObjectsManager;
            _gameStateManager = gameStateManager;
            _uIManager.AddPopup(this);

            _awaiter = _loadObjectsManager.GetObjectByPath(Constants.PATH_TO_UI_PREFABS + "Popups/ShopPopup").GetAwaiter();

            _awaiter.OnCompleted(() => 
            {
                SelfPage = MonoBehaviour.Instantiate(_awaiter.GetResult() as GameObject);
                SelfPage.transform.SetParent(_uIManager.Canvas.transform, false);
                PageComponent = SelfPage.GetComponent<ShopPopupComponent>();
                Debug.Log("ShopPopup");
                OnPopupWasInited();
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

        public void Show(object data)
        {
            Show();
        }

        public void Update()
        {

        }

        public void Dispose()
        {
            _uIManager.RemovePopup(this);
            MonoBehaviour.Destroy(SelfPage);
        }

        public void SetMainPriority()
        {
            SelfPage.transform.SetAsLastSibling();
        }

        private void MenuBtnClick()
        {
            Hide();
        }
    }

}