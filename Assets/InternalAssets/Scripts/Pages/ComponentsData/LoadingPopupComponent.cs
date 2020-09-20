using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace OrCor_GameName
{

    public class LoadingPopupComponent : MonoBehaviour
    {

        public PanelInfo Info { get { return _info; } }

        [SerializeField] private PanelInfo _info;

        [System.Serializable]
        public struct PanelInfo
        {
            public Slider sliderProgress;
            public TextMeshProUGUI progressText;

        }
    }
}