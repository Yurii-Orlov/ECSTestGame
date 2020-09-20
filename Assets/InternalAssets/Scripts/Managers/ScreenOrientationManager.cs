
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace OrCor_GameName
{
    public class ScreenOrientationManager : IInitializable, IDisposable
    {
        private CanvasScaler _uiCanvasScaler;

        private Vector2 _portraitReferenceResolution,
                        _invertReferenceResolution;

        private Enumerators.ScreenOrientationMode _currentOrientation;
        private UIManager _uIManager;

        public ScreenOrientationManager(UIManager uIManager)
        {
            _uIManager = uIManager;
        }

        public void Initialize()
        {
            _uiCanvasScaler = _uIManager.Canvas.GetComponent<CanvasScaler>();

            _portraitReferenceResolution = _uiCanvasScaler.referenceResolution;
            _invertReferenceResolution = new Vector2(_portraitReferenceResolution.y, _portraitReferenceResolution.x);

            _currentOrientation = Enumerators.ScreenOrientationMode.PORTRAIT;

        }


        public void Update()
        {
//#if UNITY_EDITOR && DEVELOPMENT
//            if (Input.GetKeyDown(KeyCode.P))
//                SwitchOrientation(Enumerators.ScreenOrientationMode.PORTRAIT);
//            else if (Input.GetKeyDown(KeyCode.L))
//                SwitchOrientation(Enumerators.ScreenOrientationMode.LANDSCAPE);
//#endif
        }



        public Enumerators.ScreenOrientationMode GetOrientation()
        {
            return _currentOrientation;
        }

        public void SwitchOrientation(Enumerators.ScreenOrientationMode mode)
        {
            switch(mode)
            {
                case Enumerators.ScreenOrientationMode.PORTRAIT:
                    _uiCanvasScaler.referenceResolution = _portraitReferenceResolution;
                    Screen.orientation = ScreenOrientation.Portrait;
                    _currentOrientation = Enumerators.ScreenOrientationMode.PORTRAIT;
                    break;
                case Enumerators.ScreenOrientationMode.LANDSCAPE:
                    _uiCanvasScaler.referenceResolution = _invertReferenceResolution;
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                    _currentOrientation = Enumerators.ScreenOrientationMode.LANDSCAPE;
                    break;

                default: break;
            }
        }

        public void Dispose()
        {
        }


    }
}