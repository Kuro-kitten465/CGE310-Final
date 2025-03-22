using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace KuroNovel.Manager
{
    public class LocalizationManager : MonoBehaviour
    {
        private Dictionary<string, string> localizedText;

        public void LoadLocalization(string language)
        {
            string path = Path.Combine(Application.streamingAssetsPath, $"dialogue_{language}.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                localizedText = JsonUtility.FromJson<Dictionary<string, string>>(json);
            }
            else
            {
                Debug.LogError($"Localization file not found: {path}");
            }
        }

        public string GetText(string key)
        {
            return localizedText.ContainsKey(key) ? localizedText[key] : key;
        }
    }
}
