using Zenject;
using System.Collections.Generic;
using UnityEngine;
using OrCor_GameName.Audio;

namespace OrCor_GameName.DataInstallers
{

    [CreateAssetMenu(fileName = "SoundChannelsData", menuName = "Installers/SoundChannelsData")]
    public class SoundChannelModelInstaller : ScriptableObjectInstaller<SoundChannelModelInstaller>
    {
        public List<SoundChannelModel> soundChannels;

        public override void InstallBindings()
        {
            Container.BindInstance(soundChannels);
        }
    }
}
