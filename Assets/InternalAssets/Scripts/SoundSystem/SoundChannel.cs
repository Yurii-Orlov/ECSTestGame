using System;
using System.Collections.Generic;
using System.Linq;
using CustomLogger;
using UniRx;
using UnityEngine;

namespace OrCor_GameName.Audio
{
    [Serializable]
    public sealed class SoundChannel : IDisposable
    {

        private readonly SoundChannelModel _soundChannelModel;

        private List<SoundSpeaker> _speakersPool = new List<SoundSpeaker>();
        private CompositeDisposable _disposables = new CompositeDisposable();
        private readonly List<SoundSpeaker> _activeSpeakers = new List<SoundSpeaker>();

        private bool _isNeedPlaySounds = true;
        private float _currentVolume;

        public SoundChannel(SoundChannelModel soundChannelModel)
        {
            _soundChannelModel = soundChannelModel;
        }

        public SoundChannelsNames SoundChannelName
        {
            get { return _soundChannelModel.ChannelName; }
        }

        public void Init(Transform speakerParent)
        {           
            var speakersHolder = new GameObject(string.Format("{0}Speakers", _soundChannelModel.ChannelName));
            speakersHolder.transform.SetParent(speakerParent);
            var speakerName = string.Format("{0}Speaker", _soundChannelModel.ChannelName);

            for (int i = 0; i < _soundChannelModel.AudioSourceLimit; i++)
            {
                GameObject soundSpeakerObj = new GameObject(speakerName);
                soundSpeakerObj.transform.SetParent(speakersHolder.transform);
                soundSpeakerObj.AddComponent<AudioSource>();
                SoundSpeaker soundSpeaker = soundSpeakerObj.AddComponent<SoundSpeaker>();
                _speakersPool.Add(soundSpeaker);
            }

            _currentVolume = SetChannelVolume();

            foreach (var item in _speakersPool)
            {
                item.GetAudioSource().volume = _currentVolume;
            }
        }

        public void Play(AudioClip audioClip, bool loop, Action onComplete = null)
        {
            var freeSpeaker = _speakersPool.FirstOrDefault(p => !p.IsActive);
            if (freeSpeaker != null)
            {
                _activeSpeakers.Add(freeSpeaker);
                freeSpeaker.IsActive = true;
                freeSpeaker.GetAudioSource().loop = loop;
                freeSpeaker.GetAudioSource().volume = SetChannelVolume();
                freeSpeaker.GetAudioSource().clip = audioClip;
                freeSpeaker.GetAudioSource().Play();
                if (!loop)
                {
                    Scheduler.MainThread.Schedule(
                        Scheduler.MainThread.Now.AddSeconds(
                            freeSpeaker.GetAudioSource().clip.length), _ =>
                    {


                        _activeSpeakers.Remove(freeSpeaker);
                        freeSpeaker.IsActive = false;

                        if (onComplete != null)
                        {
                            onComplete();
                        }
                    }).AddTo(_disposables);
                }
            }
        }

        public float SetChannelVolume()
        {
            float volume = _soundChannelModel.Volume;

            if (!_isNeedPlaySounds) return 0;

            switch (_soundChannelModel.ChannelName)
            {
                case SoundChannelsNames.Music:
                    //float musicVolume = GetSettingsData().GetBackgroundVolume() * volume;
                    //volume = musicVolume;
                    break;

                case SoundChannelsNames.Dialogs:
                    //float dialogueVolume = GetSettingsData().GetVoiceVolume() * volume;
                    //volume = dialogueVolume;
                    break;

                case SoundChannelsNames.UiSingle:
                case SoundChannelsNames.UiLoop:
                case SoundChannelsNames.Effects:
                    //float effectsVolume = GetSettingsData().GetSoundsEffectsVolume() * volume;
                    //volume = effectsVolume;
                    break;
                default:
                    break;
            }

            _currentVolume = volume;

            SetVolume(_currentVolume);

            //var settings = CoreManager.Instance.GetData<UserController>().UserSave.SettingsData;
            //var isSoundSOn = settings.GetSoundsStatus();

            //if (isSoundSOn)
            //{
            //    SetVolume(_currentVolume);
            //}
            //else
            //{
            //    SetVolume(0);
            //    return 0;
            //}

            return volume;
        }

        //private SettingsData GetSettingsData()
        //{
        //    return CoreManager.Instance.GetData<UserController>().UserSave.SettingsData;
        //}

        public bool HasAtLeastOnePlayingSource()
        {
            foreach (var s in _activeSpeakers)
            {
                if (s.GetAudioSource().isPlaying) return true;
            }

            return false;
        }

        public void StopAudioClip(AudioClip audioClip)
        {
            var playingClip = _activeSpeakers.FirstOrDefault(x => x.GetAudioSource().clip.name == audioClip.name);
            if(playingClip == null) return;
            _activeSpeakers.Remove(playingClip);
            playingClip.GetAudioSource().Stop();
            playingClip.IsActive = false;
        }

        public bool HasPlayingClip(AudioClip audioClip)
        {
            var playingClip = _activeSpeakers.FirstOrDefault(x => x.GetAudioSource().clip.name == audioClip.name);
            if(playingClip == null) return false;
            return true;
        }
        
        public void Stop()
        {
            foreach (var s in _activeSpeakers)
            {
                if(s == null) continue;
                s.GetAudioSource().Stop();
                s.IsActive = false;
            }

            if (_soundChannelModel.ChannelName == SoundChannelsNames.Music)
            {
                _disposables.Dispose();
                _disposables = new CompositeDisposable();
            }
        }

        public void Pause()
        {
            _activeSpeakers.ForEach(x => x.GetAudioSource().Pause());
        }

        public void UnPause()
        {
            _activeSpeakers.ForEach(x => x.GetAudioSource().UnPause());
        }

        public void SetVolume(float volume)
        {
            _activeSpeakers.ForEach(x => x.GetAudioSource().volume = volume);
        }

        public void SetVolumeImmediate(float volume)
        {
            if(volume == 0)
            {
                _isNeedPlaySounds = false;
            }
            _activeSpeakers.ForEach(x => x.GetAudioSource().volume = volume);
        }

        public void SetDefaultVolume()
        {
            _isNeedPlaySounds = true;
            SetChannelVolume();
            _activeSpeakers.ForEach(x => x.GetAudioSource().volume = _currentVolume);
        }

        public void SetBackVolume()
        {
            _isNeedPlaySounds = true;
            var volume = SetChannelVolume();
            _activeSpeakers.ForEach(x => x.GetAudioSource().volume = volume);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
