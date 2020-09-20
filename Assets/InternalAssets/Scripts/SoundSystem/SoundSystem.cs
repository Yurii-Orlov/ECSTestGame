using System;
using System.Collections.Generic;
using System.Linq;
using CustomLogger;
using UnityEngine;
using Zenject;

namespace OrCor_GameName.Audio
{
    public class SoundSystem : IInitializable, IDisposable
    {
        private List<SoundChannel> _channels;

        private List<SoundChannelModel> _channelModels;
        private List<SoundClipsData> _audioData;

        public SoundSystem(List<SoundChannelModel> channelModels,
                           List<SoundClipsData> soundClips)
        {
            _channelModels = channelModels;
            _audioData = soundClips;
            _channels = new List<SoundChannel>();

            var soundsHolder = new GameObject(string.Format("Sounds Holder"));
            UnityEngine.Object.DontDestroyOnLoad(soundsHolder);

            foreach (var model in _channelModels)
            {
                SoundChannel soundChannel = new SoundChannel(model);
                soundChannel.Init(soundsHolder.transform);
                _channels.Add(soundChannel);
            }
        }

        public void Initialize()
        {
            
        }

        public void Play(SoundChannelsNames soundChannelName, SoundClips soundType, bool loop, Action onComplete = null)
        {
            var targetChannel = FindChannel(soundChannelName);
            if (targetChannel == null)
            {
                return;
            }

            AudioClip audioClip = FindAudioClip(soundChannelName, soundType);

            if (audioClip == null) return;

            targetChannel.Play(audioClip, loop, onComplete);
        }

        public void StopAudioClip(SoundChannelsNames soundChannelName, AudioClip audioClip)
        {
            var targetChannel = FindChannel(soundChannelName);
            if (targetChannel == null)
            {
                return;
            }
            if (soundChannelName == SoundChannelsNames.Dialogs)
            {
                DebugLogger.Log(null, "StopAudioClip soundChannelName == SoundChannelsNames.Dialogs", LogColor.Red);
            }
            targetChannel.StopAudioClip(audioClip);
        }

        public bool HasPlayingClip(SoundChannelsNames soundChannelName, AudioClip audioClip)
        {
            var targetChannel = FindChannel(soundChannelName);
            if (targetChannel == null)
            {
                return false;
            }

            return targetChannel.HasPlayingClip(audioClip);
        }

        public void Stop(SoundChannelsNames soundChannelName)
        {
            var targetChannel = FindChannel(soundChannelName);
            if (targetChannel == null)
            {
                return;
            }
            if(soundChannelName == SoundChannelsNames.Dialogs)
            {
                DebugLogger.Log(null, "STOP soundChannelName == SoundChannelsNames.Dialogs", LogColor.Red);
            }
            targetChannel.Stop();
        }

        public bool ChannelHasPlayingSource(SoundChannelsNames soundChannelName)
        {
            var targetChannel = FindChannel(soundChannelName);
            if (targetChannel == null)
            {
                return false;
            }

            return targetChannel.HasAtLeastOnePlayingSource();
        }

        public void SetChannelVolume(SoundChannelsNames soundChannelName)
        {
            var targetChannel = FindChannel(soundChannelName);
            if (targetChannel == null)
            {
                return;
            }
            targetChannel.SetChannelVolume();
        }

        public void SetBackVolume(SoundChannelsNames soundChannelName)
        {
            var targetChannel = FindChannel(soundChannelName);
            if (targetChannel == null)
            {
                return;
            }
            targetChannel.SetBackVolume();
        }

        public void SetBackAllChannelsVolume()
        {
            _channels.ToList().ForEach(c => c.SetBackVolume());
        }

        public void SetDefaultVolume(SoundChannelsNames soundChannelName)
        {
            var targetChannel = FindChannel(soundChannelName);
            if (targetChannel == null)
            {
                return;
            }
            targetChannel.SetDefaultVolume();
        }

        public void SetDefaultAllChannelsVolume()
        {
            _channels.ToList().ForEach(c => c.SetDefaultVolume());
        }

        public void PauseAllChannels()
        {
            _channels.ToList().ForEach(c => c.Pause());
        }

        public void UnPauseAllChannels()
        {
            _channels.ToList().ForEach(c => c.UnPause());
        }

        public void SetAllChannelsVolumeImmediate(float volume)
        {
            _channels.ToList().ForEach(c => c.SetVolumeImmediate(volume));
        }

        public void SetChanellVolumeImmediate(SoundChannelsNames soundChannelName, float volume)
        {
            var targetChannel = FindChannel(soundChannelName);
            if (targetChannel == null)
            {
                return;
            }
            targetChannel.SetVolumeImmediate(volume);
        }

        private SoundChannel FindChannel(SoundChannelsNames soundChannelName)
        {
            if (_channels == null) return null;
            return _channels.FirstOrDefault(c => c.SoundChannelName == soundChannelName);
        }

        private AudioClip FindAudioClip(SoundChannelsNames soundChannelName, SoundClips soundClip)
        {
            SoundClipsData channelSounds = _audioData.FirstOrDefault(p => p.ChannelName == soundChannelName);

            if (channelSounds == null)
            {
                DebugLogger.Log(null, "No audio clips for channel =>> " + soundChannelName, LogColor.Red);
                return null;
            }

            SoundClip soundClipData = channelSounds.SoundData.FirstOrDefault(p => p.Type == soundClip);

            if (soundClipData == null)
            {
                DebugLogger.Log(null, "No audio clip for sound type ==> " + soundClip, LogColor.Red);
                return null;
            }

            return soundClipData.Audio;
        }

        public void Dispose()
        {
            if(_channels != null && _channels.Any())
            {
                _channels.ToList().ForEach(c => c.Dispose());
            }
        }
    }
}
