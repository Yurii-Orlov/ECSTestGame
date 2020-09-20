using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using System;

namespace OrCor_GameName
{
    public class UIManager : IInitializable, IDisposable
    {

        public List<IUIElement> Pages { get; private set; }

        public List<IUIPopup> Popups { get; private set; }

        public IUIElement CurrentPage { get; set; }

        public CanvasScaler CanvasScaler { get; set; }
        public GameObject Canvas { get; set; }

        private GameStateManager _gameStateManager;

        public UIManager(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
            Pages = new List<IUIElement>();

            Popups = new List<IUIPopup>();

            GameObject uiView = MonoBehaviour.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/[UIView]"));
            Canvas = uiView.transform.Find("Canvas").gameObject;
        }


        public void Initialize()
        {
            
            Canvas.transform.parent.SetParent(_gameStateManager.transform.parent);
            CanvasScaler = Canvas.GetComponent<CanvasScaler>();

            Observable.OnceApplicationQuit()
                .Subscribe(x =>
                {
                    Dispose();
                });
        }

        public void AddPage(IUIElement page)
        {
            Pages.Add(page);
        }

        public void AddPopup(IUIPopup popup)
        {
            Popups.Add(popup);
        }

        public void RemovePage(IUIElement page)
        {
            Pages.Remove(page);       
        }

        public void RemovePopup(IUIPopup popup)
        {

             Popups.Remove(popup);
        }


        public void HideAllPages()
        {
            foreach (var _page in Pages)
            {
                _page.Hide();
            }
        }

        public void HideAllPopups()
        {
            foreach (var _popup in Popups)
            {
                _popup.Hide();
            }
        }

        public void SetPage<T>(bool hideAll = false) where T : IUIElement
        {
            if (hideAll)
            {
                HideAllPages();
            }
            else
            {
                if (CurrentPage != null)
                    CurrentPage.Hide();
            }

            foreach (var _page in Pages)
            {
                if (_page is T)
                {
                    CurrentPage = _page;
                    break;
                }
            }
            CurrentPage.Show();
        }

        public void DrawPopup<T>(object message = null, bool setMainPriority = false) where T : IUIPopup
        {
            IUIPopup popup = null;
            foreach (var _popup in Popups)
            {
                if (_popup is T)
                {
                    popup = _popup;
                    break;
                }
            }

            if (setMainPriority)
                popup.SetMainPriority();

            if (message == null)
                popup.Show();
            else
                popup.Show(message);
        }

        public void HidePopup<T>() where T : IUIPopup
        {
            foreach (var _popup in Popups)
            {
                if (_popup is T)
                {
                    _popup.Hide();
                    break;
                }
            }
        }

        public IUIPopup GetPopup<T>() where T : IUIPopup
        {
            IUIPopup popup = null;
            foreach (var _popup in Popups)
            {
                if (_popup is T)
                {
                    popup = _popup;
                    break;
                }
            }

            return popup;
        }

        public IUIElement GetPage<T>() where T : IUIElement
        {
            IUIElement page = null;
            foreach (var _page in Pages)
            {
                if (_page is T)
                {
                    page = _page;
                    break;
                }
            }

            return page;
        }


        private void Dispose()
        {
            //foreach (var page in Pages)
            //    page.Dispose();

            //foreach (var popup in Popups)
            //    popup.Dispose();
        }

        void IDisposable.Dispose()
        {
           
        }
    }
}