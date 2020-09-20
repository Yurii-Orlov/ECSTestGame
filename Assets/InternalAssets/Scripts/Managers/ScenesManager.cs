using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Zenject;
using UniRx;
using System;
using CustomLogger;

namespace OrCor_GameName
{
    public sealed class ScenesManager : IInitializable
    {
        public event Action<float> OnSceneLoading = delegate (float progress) { };
        public event Action OnSceneFinishedLoading = delegate () { };

        public static string CurrentSceneName { get; private set; }

        public bool IsLoadedScene { get; set; }

        private LoadingPopupComponent _loadingComponent;
        private readonly GameStateManager _gameStateManager;
        private readonly UIManager _uIManager;

        public ScenesManager(GameStateManager gameStateManager, UIManager uIManager)
        {

            _gameStateManager = gameStateManager;
            _uIManager = uIManager;
        }

        public void Initialize()
        {
 
        }


        public void ChangeScene(string sceneName, bool isLoadAsync)
        {
            CurrentSceneName = sceneName;
            IsLoadedScene = false;

            if (isLoadAsync)
            {
                _uIManager.DrawPopup<LoadingPopup>(setMainPriority: true);
                SceneManager.LoadSceneAsync(sceneName) // загружаем асинхронно сцену
                    .AsAsyncOperationObservable() // превращаем его в Observable поток
                    .Do(x =>
                    { // вызывается при выполнении процесса
                        float progress = Mathf.Clamp01(x.progress / 0.9f);
                        DebugLogger.Log(null, "Loading progress = " + progress, LogColor.Aqua);
                        OnSceneLoading(progress);
                    }).Subscribe(_ =>
                    { // подписываемся
                        
                        Observable.Timer(System.TimeSpan.FromSeconds(0.5f)).Subscribe(p =>
                        {
                            OnSceneFinishedLoading();
                        });
                    });

            }
            else
            {
                SceneManager.LoadScene(sceneName);
                OnSceneFinishedLoading();
            }


        }


        public void Dispose()
        {
            
        }


    }
}