using OrCor_GameName;
using OrCor_GameName.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OrCor_GameName
{

    public class GameSceneBindings : MonoInstaller
    {
        public GameObject EnemyPrefab;

        public override void InstallBindings()
        {
            InitServices();
            InitUiViews();
            InitPool();
        }

        private void InitServices()
        {
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.Bind(typeof(IInitializable), typeof(IDisposable)).To<SpawnerController>().AsSingle();
            Container.BindFactory<PlayerContoller, PlayerContoller.Factory>();

        }

        private void InitUiViews()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IUIElement)).To<GamePage>().AsSingle();
        }

        private void InitPool()
        {
            Container.BindFactory<EnemyFacade, EnemyFacade.Factory>()
           .FromMonoPoolableMemoryPool<EnemyFacade>(b => b
               .WithInitialSize(2)
               .FromSubContainerResolve()
               .ByNewPrefabMethod(EnemyPrefab, InstallEnemy)
               .UnderTransformGroup("Enemies"));
        }

        static void InstallEnemy(DiContainer subContainer)
        {
            subContainer.Bind<EnemyFacade>().FromNewComponentOnRoot().AsSingle();
            subContainer.Bind<PoolableManager>().AsSingle();
        }

    }

}
