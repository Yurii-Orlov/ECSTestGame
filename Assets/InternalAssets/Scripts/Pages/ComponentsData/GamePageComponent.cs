using UnityEngine.UI;
using UnityEngine;

namespace OrCor_GameName
{

    public class GamePageComponent : MonoBehaviour
    {

        public PanelInfo Info { get { return _info; } }

        [SerializeField] private PanelInfo _info;

        [System.Serializable]
        public struct PanelInfo
        {
            public Button menuBtn;

        }
    }
}