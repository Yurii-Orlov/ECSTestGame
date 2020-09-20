using Zenject;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace OrCor_GameName
{
    public class LoadingPopup : IInitializable, IDisposable, IUIPopup
    {
        public event Action OnPopupWasInited = delegate { };

        public GameObject SelfPage { get; private set; }

        private readonly LoadObjectsManager _loadObjectsManager;
        private readonly UIManager _uiManager;
        private readonly ScenesManager _scenesManager;

        private LoadingPopupComponent _pageComponent;

        public LoadingPopup(LoadObjectsManager loadObjectsManager,
                            UIManager uiManager,
                            ScenesManager scenesManager)
        {
            _loadObjectsManager = loadObjectsManager;
            _uiManager = uiManager;
            _uiManager.AddPopup(this);
            _scenesManager = scenesManager;
            var resourceObject = _loadObjectsManager.GetObjectByPath(Constants.PATH_TO_UI_PREFABS + "Popups/LoadingPopup").GetAwaiter();

            resourceObject.OnCompleted(() => {
                SelfPage = MonoBehaviour.Instantiate(resourceObject.GetResult() as GameObject);
                SelfPage.transform.SetParent(_uiManager.Canvas.transform, false);
                _pageComponent = SelfPage.GetComponent<LoadingPopupComponent>();
                OnPopupWasInited();
            });


        }

        public void Initialize()
        {
            Hide();

            _scenesManager.OnSceneLoading += LoadingPopup_OnSceneLoading;
            _scenesManager.OnSceneFinishedLoading += LoadingPopup_OnSceneFinishedLoading;
        }

        private void LoadingPopup_OnSceneFinishedLoading()
        {
            Hide();
        }

        private void LoadingPopup_OnSceneLoading(float progress)
        {
            Debug.Log("Scene loading ");
            _pageComponent.Info.sliderProgress.value = progress;
            _pageComponent.Info.progressText.text = (progress * 100f).ToString() + " %";
        }

        public void SetMainPriority()
        {
            SelfPage.transform.SetAsLastSibling();
        }

        public void Show()
        {
            _pageComponent.Info.sliderProgress.value = 0;
            SelfPage.SetActive(true);
        }

        public void Show(object data)
        {
            Show();
        }

        public void Update(){}

        public void Dispose()
        {
            _uiManager.RemovePopup(this);
            MonoBehaviour.Destroy(SelfPage);
            //this.Dispose();
        }

        public void Hide()
        {
            SelfPage.SetActive(false);
        }
    }
}
