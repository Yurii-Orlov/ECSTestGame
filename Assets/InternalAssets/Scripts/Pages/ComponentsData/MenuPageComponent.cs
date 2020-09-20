using UnityEngine.UI;
using UnityEngine;


namespace OrCor_GameName
{

    public class MenuPageComponent : MonoBehaviour
    {

        public PanelInfo Info { get { return _info; } }

        [SerializeField] private PanelInfo _info;

        [System.Serializable]
        public struct PanelInfo
        {
            public Button playBtn;
            public Button settingsBtn;
            public Button shopBtn;
            public Button testBtn;
            public Text testCoins;
        }
    }
}