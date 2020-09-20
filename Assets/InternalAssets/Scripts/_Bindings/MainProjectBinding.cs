using Zenject;
using UnityEngine;
using System;
using OrCor_GameName.Audio;

namespace OrCor_GameName
{

    public class MainProjectBinding : MonoInstaller
    {
        public override void InstallBindings()
        {
            InitServices();
            InitUiViews();
            InstallGameStateManager();
            InstallGameObjects();
        }

        private void InitServices()
        {
            Container.BindInterfacesAndSelfTo<PlayerDataManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScreenOrientationManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScenesManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadObjectsManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<LocalizationManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<MobileKeyboardManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<TimerManager>().AsSingle();
        }

        private void InitUiViews()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IUIPopup)).To<LoadingPopup>().AsSingle();
        }

        private void InstallGameStateManager()
        {
            Container.Bind<GameStateFactory>().AsSingle();

            Container.BindFactory<GameState, GameState.Factory>().WhenInjectedInto<GameStateFactory>();
            Container.BindFactory<MenuState, MenuState.Factory>().WhenInjectedInto<GameStateFactory>();
        }

        private void InstallGameObjects()
        {
            Container.Bind<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateManager>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }

}
