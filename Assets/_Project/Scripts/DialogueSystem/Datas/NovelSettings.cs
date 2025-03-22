using System;
using UnityEngine;

namespace KuroNovel.Data
{
    [CreateAssetMenu(fileName = "NovelSettings", menuName = "KuroNovel/Settings")]
    public class NovelSettings : ScriptableObject
    {
        public float TextSpeed = 0.05f;
        public float AutoReadSpeed = 2f;
        public Color DefaultTextColor = Color.white;
        public int DefaultFontSize = 24;
        public Color DefaultBackgroundColor = Color.black;
        public DialogueBoxSettings DialogueBoxSettings = new DialogueBoxSettings();
        public DefaultTransitions DefaultTransitions = new DefaultTransitions();
    }

    [Serializable]
    public class DialogueBoxSettings
    {
        public float BoxOpacity = 0.8f;
        public Color BoxColor = new Color(0, 0, 0, 0.8f);
        public Color NameBoxColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        public float BorderSize = 2f;
        public Color BorderColor = Color.white;
    }
}
