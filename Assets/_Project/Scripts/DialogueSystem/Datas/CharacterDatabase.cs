using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KuroNovel.Data
{
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "KuroNovel/Character Database")]
    public class CharacterDatabase : ScriptableObject
    {
        public List<CharacterData> Characters = new List<CharacterData>();
    }

    [CreateAssetMenu(fileName = "CharacterData", menuName = "KuroNovel/Character")]
    public class CharacterData : ScriptableObject
    {
        public string ID;
        //public LocalizedString DisplayName;
        [TextArea(2, 5)]
        public string Description;
        public List<EmotionState> EmotionStates = new List<EmotionState>();
        public Color NameColor = Color.white;
        public VoiceSettings VoiceSettings = new VoiceSettings();
    }

    [Serializable]
    public class EmotionState
    {
        public string ID;
        public AssetReferenceSprite SpriteReference;
        public string AnimationOverride;
    }

    [Serializable]
    public class VoiceSettings
    {
        public float Pitch = 1f;
        public float Volume = 1f;
    }
}
