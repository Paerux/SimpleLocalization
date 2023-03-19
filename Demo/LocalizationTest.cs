using System;
using System.Collections;
using System.Collections.Generic;
using Paerux.Localization;
using UnityEngine;

public class LocalizationTest : MonoBehaviour
{
   private LocalizationManager manager;
   [SerializeField] private string key;
   [SerializeField] private string language;

   private void Awake()
   {
      manager = GetComponent<LocalizationManager>();
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.T))
      {
         Debug.Log(manager.GetTranslation(key));
      }

      if (Input.GetKeyDown(KeyCode.R))
      {
         Debug.Log(manager.GetSpecificTranslation(key, language));
      }

      if (Input.GetKeyDown(KeyCode.C))
      {
         manager.SwitchLanguage(language);
      }
   }
}
