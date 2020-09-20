using OrCor_GameName.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OrCor_GameName.DataInstallers {

    [CreateAssetMenu(fileName = "SoundClipsDataInstaller", menuName = "Installers/SoundClipsData")]
    public class SoundClipsDataInstaller : ScriptableObjectInstaller<SoundClipsDataInstaller>
    {
        public List<SoundClipsData> audioData;

        public override void InstallBindings()
        {
            Container.BindInstance(audioData);
        }

    }

}