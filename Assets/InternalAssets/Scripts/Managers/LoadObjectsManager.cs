using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;
using UniRx.Async;

namespace OrCor_GameName
{
    public class LoadObjectsManager : IInitializable
    {
        private bool _loadFromResources = true;


        public LoadObjectsManager()
        {

        }

        public void Initialize()
        {

        }


        public async UniTask<UnityEngine.Object> GetObjectByPath(string path)
        {
             return await LoadFromResourcesAsync(path);
        }

        public string GetTextByPath(string path)
        {
            return File.ReadAllText(path);
        }


        public void SetTextByPath(string path, string data)
        {
            File.WriteAllText(path, data);
        }

        private T LoadFromResources<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        private async UniTask<UnityEngine.Object> LoadFromResourcesAsync(string path)
        {
            var resource = await Resources.LoadAsync<UnityEngine.Object>(path);

            return (resource as UnityEngine.Object);
        }


    }
}