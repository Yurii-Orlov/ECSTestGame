using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace OrCor_GameName
{
    public class MobileKeyboardManager : IInitializable
    {
        public event Action KeyboardDoneEvent = delegate() { };
        public event Action KeyboardCanceledEvent = delegate() { };

        private TouchScreenKeyboard _currentKeyboard;
        private CompositeDisposable _disposable;

        public MobileKeyboardManager()
        {

        }


        public void Initialize()
        {
            _disposable = new CompositeDisposable();

            Observable.EveryUpdate().
                Subscribe(x =>
                {
                    Update();
                }).AddTo(_disposable);

            Observable.OnceApplicationQuit().
                Subscribe(x =>
                {
                    _disposable.Dispose();
                });
        }

        public void Update()
        {
            if (_currentKeyboard != null)
            {
                if (!TouchScreenKeyboard.visible && _currentKeyboard.status == TouchScreenKeyboard.Status.Canceled)
                    HideKeyboard(false, true);
                else if (!TouchScreenKeyboard.visible && _currentKeyboard.status == TouchScreenKeyboard.Status.Done && _currentKeyboard.status != TouchScreenKeyboard.Status.Canceled)
                    HideKeyboard(true, false);
            }
        }

        public void DrawKeyboard(string text, bool hideInput = false, TouchScreenKeyboardType keyboardType = TouchScreenKeyboardType.Default, bool autocorrection = true, bool multiline = false, bool secure = false)
        {
            HideKeyboard(false, false);

            TouchScreenKeyboard.hideInput = hideInput;

            _currentKeyboard = TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, false, string.Empty);
        }

        public void HideKeyboard(bool throwDoneEvent, bool throwCanceledEvent)
        {
            if (_currentKeyboard == null)
                return;

            _currentKeyboard.active = false;

            _currentKeyboard = null;

            if (throwDoneEvent)
            {
                 KeyboardDoneEvent();
            }

            if (throwCanceledEvent)
            {
                KeyboardCanceledEvent();
            }
        }

    }
}