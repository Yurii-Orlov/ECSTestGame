using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OrCor_GameName
{
    public class MenuSceneBindings : MonoInstaller
    {
        public override void InstallBindings()
        {
            InitServices();
            InitUiViews();
        }

        private void InitServices()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable)).To<ShopManager>().AsSingle();
        }

        private void InitUiViews()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IUIElement)).To<MenuPage>().AsSingle();
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IUIPopup)).To<ShopPopup>().AsSingle();
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IUIPopup)).To<SettingsPopup>().AsSingle();
        }
    }
}
