using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace OrCor_GameName
{
    public class LocalizationManager : IInitializable
    {
        public event Action<Enumerators.Language> LanguageWasChangedEvent;
        public Dictionary<SystemLanguage, Enumerators.Language> SupportedLanguages { get; private set; }

        public Enumerators.Language CurrentLanguage { get { return _currentLanguage; } }

        private PlayerDataManager _dataManager;


        private Enumerators.Language _defaultLanguage = Enumerators.Language.DE,
                                     _currentLanguage = Enumerators.Language.NONE;



        public LocalizationManager(PlayerDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public void Initialize()
        {
            FillLanguages();
            ApplyLocalization();
        }


        public void ApplyLocalization()
        {
            if (!SupportedLanguages.ContainsKey(Application.systemLanguage))
            {
                if (_dataManager.UserSave.AppLanguage == Enumerators.Language.NONE)
                    SetLanguage(_defaultLanguage);
                else
                {
                    SetLanguage(_dataManager.UserSave.AppLanguage);
                }
            }
            else
            {
                if (_dataManager.UserSave.AppLanguage == Enumerators.Language.NONE)
                    SetLanguage(SupportedLanguages[Application.systemLanguage]);
                else
                    SetLanguage(_dataManager.UserSave.AppLanguage);
            }
        }


        public void SetLanguage(Enumerators.Language language, bool forceUpdate = false)
        {
            if (language == CurrentLanguage && !forceUpdate)
                return;

            string languageCode = language.ToString().ToLower();

            I2.Loc.LocalizationManager.SetLanguageAndCode(I2.Loc.LocalizationManager.GetLanguageFromCode(languageCode), languageCode);

            _currentLanguage = language;
            _dataManager.UserSave.AppLanguage = language;

            LanguageWasChangedEvent?.Invoke(_currentLanguage);
        }

        public string GetUITranslation(string key)
        {
            return I2.Loc.LocalizationManager.GetTermTranslation(key);
        }


        private void FillLanguages()
        {
            SupportedLanguages = new Dictionary<SystemLanguage, Enumerators.Language>();

#if UNITY_EDITOR
            SupportedLanguages.Add(SystemLanguage.Ukrainian, Enumerators.Language.UK);
#endif

            SupportedLanguages.Add(SystemLanguage.English, Enumerators.Language.EN);
            SupportedLanguages.Add(SystemLanguage.German, Enumerators.Language.DE);
        }


    }
}