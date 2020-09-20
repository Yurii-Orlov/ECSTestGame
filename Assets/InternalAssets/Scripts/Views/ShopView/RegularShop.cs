using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using OrCor_GameName.Utils;

namespace OrCor_GameName
{
    [System.Serializable]
    public struct ShopItemData
    {
        public int id;
        public Enumerators.ShopReapeatType reapeatType;
        public string shopTitle;
        public int shopPrice;
        public Sprite shopSprite;
    }

    public class RegularShop : IShop
    {
        private List<RegularShopItem> _shopList;
        private GameObject _shopPrefab;
        private LoadObjectsManager _loadObjectsManager;
        private Transform _parent;

        public RegularShop(LoadObjectsManager loadObjectsManager, Transform parent)
        {
            _parent = parent;
            _loadObjectsManager = loadObjectsManager;


        }

        public void InitShop()
        {
            var _shopData = DataShopUtils.GetDataShopRecords().Select(r => new ShopItemData()
            {
                id = r.Id,
                reapeatType = r.RepeatType,
                shopPrice = r.Price,
                shopSprite = r.ShopSprite,
                shopTitle = r.Title
            }).ToList();

            _shopList = new List<RegularShopItem>();

            var resourceObject = _loadObjectsManager.GetObjectByPath(Constants.PATH_TO_UI_PREFABS + "Shop/" + "Shop_Item").GetAwaiter();

            resourceObject.OnCompleted(() => {
                _shopPrefab = resourceObject.GetResult() as GameObject;

                for (int i = 0; i < _shopData.Count; i++)
                {
                    RegularShopItem item = new RegularShopItem(_parent, _shopPrefab, _shopData[i]);
                    item.CreateShopItem();
                    _shopList.Add(item);
                }
            });

        }

        public void UpdateShop()
        {
            
        }
    }

    public class RegularShopItem
    {
        private Transform _parent;
        private GameObject _prefab;
        private ShopItemData _shopItemData;
        private GameObject _shopObj;
        private ShopItemView.ShopInfoStruct _shopInfoStruct;

        public RegularShopItem(Transform parent, GameObject prefab, ShopItemData shopItemData)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
            _shopItemData = shopItemData;
        }

        /// <summary>
        /// Create shop item and parent it to UI
        /// </summary>
        public void CreateShopItem()
        {
            _shopObj = MonoBehaviour.Instantiate(_prefab);
            _shopObj.transform.SetParent(_parent);
            _shopObj.transform.localScale = Vector3.one;

            _shopInfoStruct = _shopObj.GetComponent<ShopItemView>().ShopInfo;

            if(_shopInfoStruct.selfImage) _shopInfoStruct.selfImage.sprite = _shopItemData.shopSprite;
            if(_shopInfoStruct.txtPrice) _shopInfoStruct.txtPrice.text = _shopItemData.shopPrice.ToString();
            if(_shopInfoStruct.txtTitle) _shopInfoStruct.txtTitle.text = _shopItemData.shopTitle;
            if(_shopInfoStruct.selfButton) _shopInfoStruct.selfButton.onClick.AddListener(OnBtnClickListener);
        }

        private void OnBtnClickListener()
        {

        }
    }

}