
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OrCor_GameName
{

    public class ShopItemView : MonoBehaviour
    {
        public ShopInfoStruct ShopInfo { get { return _shopInfo; } }

        [SerializeField] private ShopInfoStruct _shopInfo;

        [System.Serializable]
        public struct ShopInfoStruct
        {
            public TextMeshProUGUI txtPrice;
            public TextMeshProUGUI txtTitle;
            public Image selfImage;
            public Button selfButton;
        }
    }

}