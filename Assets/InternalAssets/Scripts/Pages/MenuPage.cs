using System;
using UnityEngine;
using UniRx;
using CustomLogger;
using Zenject;
using OrCor_GameName.Audio;
using UniRx.Async;

namespace OrCor_GameName
{

    public class MenuPage : IInitializable, IDisposable, IUIElement
    {
        public GameObject SelfPage { get; set; }

        private readonly UIManager _uiManager;
        private readonly LoadObjectsManager _loadObjectsManager;
        private readonly ScenesManager _scenesManager;
        private readonly GameStateManager _gameStateManager;
        private readonly PlayerDataManager _dataManager;
        private readonly SoundSystem _soundSystem;

        private MenuPageComponent _pageComponent;
        private CompositeDisposable _disposables = new CompositeDisposable();
        private UniTask<UnityEngine.Object>.Awaiter _awaiter;

        public MenuPage(UIManager uiManager,
                        LoadObjectsManager loadObjectsManager,
                        ScenesManager scenesManager,
                        GameStateManager gameStateManager,
                        PlayerDataManager dataManager,
                        SoundSystem soundSystem)
        {
            _uiManager = uiManager;
            _loadObjectsManager = loadObjectsManager;
            _scenesManager = scenesManager;
            _gameStateManager = gameStateManager;
            _dataManager = dataManager;
            _soundSystem = soundSystem;

            _uiManager.AddPage(this);

            _awaiter = _loadObjectsManager.GetObjectByPath(Constants.PATH_TO_UI_PREFABS + "MenuPage").GetAwaiter();



            _awaiter.OnCompleted(() => {
                SelfPage = MonoBehaviour.Instantiate(_awaiter.GetResult() as GameObject);
                SelfPage.transform.SetParent(_uiManager.Canvas.transform, false);
                _pageComponent = SelfPage.GetComponent<MenuPageComponent>();
            });

        }

        public void Initialize()
        {
            _awaiter.OnCompleted(() => {
                _pageComponent.Info.playBtn.onClick.AddListener(PlayBtnClick);
                _pageComponent.Info.settingsBtn.onClick.AddListener(SettingsBtnClick);
                _pageComponent.Info.shopBtn.onClick.AddListener(ShopBtnClick);
                _pageComponent.Info.testBtn.onClick.AddListener(TestBtnClick);
                _dataManager.UserSave.Coins.SubscribeToText(_pageComponent.Info.testCoins).AddTo(SelfPage);
                //_gameClient.Get<DataManager>().UserSave.Coins.ObserveEveryValueChanged(x => x.Value)
                //                                             .Subscribe(xs => {
                //                                                 _pageComponent.Info.testCoins.text = xs.ToString();
                //                                             }).AddTo(_disposables);

                Hide();
            });
        }


        private void TestBtnClick()
        {
            _dataManager.UserSave.Coins.Value += 5;
            _soundSystem.Play(SoundChannelsNames.UiSingle, SoundClips.CLICK, false);
        }

        public void AfterInit()
        {

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

        private void ShopBtnClick()
        {
            _uiManager.DrawPopup<ShopPopup>(setMainPriority: true);
        }

        private void SettingsBtnClick()
        {
            _uiManager.DrawPopup<SettingsPopup>(setMainPriority: true);
        }

        private void PlayBtnClick()
        {
            _gameStateManager.ChangeState(Enumerators.GameStateTypes.START_GAMEPLAY);
        }
    }
}
