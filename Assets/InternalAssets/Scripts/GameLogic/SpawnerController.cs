
using CustomLogger;
using OrCor_GameName.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OrCor_GameName {

    public class SpawnerController : IInitializable, IDisposable
    {
        private readonly PlayerContoller.Factory _playerControllerFactory;
        private readonly EnemyFacade.Factory _enemyControllerFactory;
        private readonly TestSettings _testSettings;

        public SpawnerController(PlayerContoller.Factory playerControllerFactory,
                                 EnemyFacade.Factory enemyControllerFactory,
                                 TestSettings testSettings)
        {
            _playerControllerFactory = playerControllerFactory;
            _enemyControllerFactory = enemyControllerFactory;
            _testSettings = testSettings;
        }

        public void Initialize()
        {
            PlayerContoller player = _playerControllerFactory.Create();

            for (int i = 0; i < _testSettings.EnemiesCreateCount; i++)
            {
                EnemyFacade enemy = _enemyControllerFactory.Create();
            }
        }

        public void Dispose()
        {
            DebugLogger.Log(null, "Dispose SpawnerController", LogColor.Grey);
        }
    }

}