using Common.Serialization;
using Core.ModelManagement;
using Core.SaveData;
using CustomLogger;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;
using UniRx;

namespace OrCor_GameName
{
    public class PlayerDataManager : IInitializable, IDisposable
    {
        public UserSave UserSave;
        public static ModelController ModelContr { get; private set; }
        public static SerializedController SerializedController { get; private set; }

        private LocalizationManager _localizationManager;

        public PlayerDataManager()
        {
            if (ModelContr == null)
            {
                ModelContr = new ModelController();
            }

            SerializedController = new SerializedController();

            UserSave = new UserSave();

            var readSaveData = UserSave.Read();
            DebugLogger.Log(null, "[Init] save data exists: " + (readSaveData != null));
            UserSave = (UserSave)(readSaveData ?? UserSave);

            UserSave.Init();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;
#endif

            Observable.OnceApplicationQuit().Subscribe(_ => OnceApplicationQuit());
            Observable.EveryApplicationPause().Subscribe(_ => OnApplicationPause());
 
        }

        public void Initialize()
        {

            //_localizationManager = _gameClient.Get<LocalizationManager>();
            //if (_localizationManager != null) _localizationManager.ApplyLocalization();
        }

        private void OnApplicationPause()
        {
            ModelContr.WriteAll();
        }

        private void OnceApplicationQuit()
        {
            ModelContr.WriteAll();
        }

#if UNITY_EDITOR
        private static void EditorApplicationOnPlayModeStateChanged(UnityEditor.PlayModeStateChange playModeStateChange)
        {
            var stopPlay = playModeStateChange == UnityEditor.PlayModeStateChange.ExitingPlayMode;
            if (!stopPlay) return;

            DebugLogger.Log(null, "EditorApplicationOnPlayModeStateChanged", LogColor.Red);

            ModelContr.WriteAll();
            ModelContr.UnRegisterAll();
        }
#endif

        public void Dispose()
        {
            ModelContr.WriteAll();
        }


    }
}