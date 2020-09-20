using UnityEngine;

namespace OrCor_GameName.Audio
{
    public class SoundSpeaker : MonoBehaviour
    {
        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                _isActive = value;
                gameObject.SetActive(_isActive);
            }
        }

        [SerializeField] private AudioSource _audioSource;

        private bool _isActive;

        private void Awake()
        {
            _audioSource = _audioSource ?? GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            IsActive = false;
        }

        public AudioSource GetAudioSource()
        {
            return _audioSource;
        } 
        
    }
}
