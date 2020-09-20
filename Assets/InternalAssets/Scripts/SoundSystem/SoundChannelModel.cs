using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrCor_GameName.Audio
{
    [System.Serializable]
    public class SoundChannelModel
    {
        public SoundChannelsNames ChannelName;
        public int AudioSourceLimit = 1;
        public float Volume = 1.0f;
    }

}