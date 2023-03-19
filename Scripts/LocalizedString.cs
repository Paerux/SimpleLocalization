using System;
using System.Collections;
using System.Collections.Generic;
using Paerux.Localization;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedString : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField] private string key;
    [SerializeField] private LocalizationManager localizationManager;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    
    private void OnEnable()
    {
        localizationManager.OnLanguageChange.AddListener(OnLanguageChange);
        localizationManager.OnInitialized.AddListener(OnLanguageChange);
    }

    private void OnDisable()
    {
        localizationManager.OnLanguageChange.RemoveListener(OnLanguageChange);
        localizationManager.OnInitialized.RemoveListener(OnLanguageChange);
    }

    private void OnLanguageChange()
    {
        text.text = localizationManager.GetTranslation(key);
    }
}
