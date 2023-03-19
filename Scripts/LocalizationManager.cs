using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Paerux.Localization
{
    public class LocalizationManager:MonoBehaviour

    {
        [SerializeField] private AssetReference localizationAssetReference;
        public bool translationReady;
        public string currentLanguage = "English";
        public string[] availableLanguages;
        public UnityEvent OnLanguageChange;
        public UnityEvent OnInitialized;
        private Dictionary<string, Dictionary<string, string>> translationTable;


        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if(!translationReady)
                LoadTranslationFile();
        }

        private void LoadTranslationFile()
        {
            Addressables.LoadAssetAsync<TextAsset>(localizationAssetReference).Completed +=
                (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        var file = asyncOperationHandle.Result;
                        translationReady = FillDictionary(file);
                        if(translationReady) OnInitialized?.Invoke();
                    }
                    else
                    {
                        translationReady = false;
                    }
                };
        }

        private bool FillDictionary(TextAsset file)
        {
            try
            {
                translationTable = new Dictionary<string, Dictionary<string, string>>();
                var data = JsonUtility.FromJson<LocalizationData>(file.text);
                availableLanguages = data.available_languages;
                foreach (var keyTranslation in data.table)
                {
                    translationTable[keyTranslation.key] = new Dictionary<string, string>();
                    foreach (var languageValue in keyTranslation.translations)
                    {
                        Debug.Log($"{keyTranslation.key} - {languageValue.language} - {languageValue.value}");
                        translationTable[keyTranslation.key].Add(languageValue.language, languageValue.value);
                    }
                }

                var savedLang = PlayerPrefs.GetString("savedLanguage","English");
                if (!availableLanguages.Contains(savedLang))
                    savedLang = "English";

                currentLanguage = savedLang;
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }

        public string GetTranslation(string key)
        {
            if (!translationReady)
            {
                Initialize();
                return "ERROR";
            }

            if (!translationTable.ContainsKey(key))
            {
                Debug.LogError("Translation file doesn't contain the key");
                return "ERROR";
            }

            return translationTable[key][currentLanguage];
        }

        public string GetSpecificTranslation(string key, string language)
        {
            if (!translationReady)
            {
                Initialize();
                return "ERROR";
            }

            if (availableLanguages.Contains(language)) return translationTable[key][language];

            Debug.LogError("Specified language is not available");
            return GetTranslation(key);
        }

        public void SwitchLanguage(string language)
        {
            if (!availableLanguages.Contains(language))
            {
                Debug.LogError("Specified language is not available");
                return;
            }

            currentLanguage = language;
            OnLanguageChange?.Invoke();
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.SetString("savedLanguage",currentLanguage);
            PlayerPrefs.Save();
        }
    }
}