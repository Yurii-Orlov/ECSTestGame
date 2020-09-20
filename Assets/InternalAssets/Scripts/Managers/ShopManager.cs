using Zenject;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace OrCor_GameName
{
    public class ShopManager : IInitializable, IDisposable
    {
        private ShopPopupComponent _shopComponent;

        private IShop _regularShop;
        private readonly UIManager _uIManager;
        private readonly LoadObjectsManager _loadObjectsManager;

        public ShopManager(UIManager uIManager, LoadObjectsManager loadObjectsManager)
        {
            _uIManager = uIManager;
            _loadObjectsManager = loadObjectsManager;
        }

        public void Initialize()
        {
            var shop = _uIManager.GetPopup<ShopPopup>();
            if( shop == null)
            {
                return;
            }

            shop.OnPopupWasInited += delegate 
            {
                Debug.Log("ShopManager");
                _shopComponent = _uIManager.GetPopup<ShopPopup>().SelfPage.GetComponent<ShopPopupComponent>();

                _regularShop = new RegularShop(_loadObjectsManager, _shopComponent.Info.parentForShopCenterItems);

                InitShop();
            };


        }

        public void UpdateShop()
        {
            _regularShop.UpdateShop();
        }

        public void Dispose()
        {

        }

        private void InitShop()
        {
            _regularShop.InitShop();
        }
    }

}
