using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrCor_GameName.Audio
{
    [System.Serializable]
    public class SoundClipsData 
    {
        public SoundChannelsNames ChannelName;
        public List<SoundClip> SoundData;
    }

    [System.Serializable]
    public class SoundClip
    {
        public SoundClips Type;
        public AudioClip Audio;
    }

}