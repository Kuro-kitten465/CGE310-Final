using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;

namespace KuroNovel.Data
{
    [CreateAssetMenu(fileName = "BackgroundDatabase", menuName = "KuroNovel/Background Database")]
    public class BackgroundDatabase : ScriptableObject
    {
        public List<BackgroundData> Backgrounds = new List<BackgroundData>();
    }

    [CreateAssetMenu(fileName = "BackgroundData", menuName = "KuroNovel/Background")]
    public class BackgroundData : ScriptableObject
    {
        public string ID;
        public string DisplayName;
        public AssetReferenceSprite SpriteReference;
        public AssetReferenceAudio MusicReference;
        public AssetReferenceAudio AmbientSoundReference;
        public List<TransitionEffect> TransitionEffects = new List<TransitionEffect>();
    }

    [Serializable]
    public class TransitionEffect
    {
        public string ID;
        public float Duration = 1f;
        public AnimationCurve AnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }

    [Serializable]
    public class DefaultTransitions
    {
        public string SceneTransition = "Fade";
        public string CharacterTransition = "Fade";
        public string DialogueTransition = "Fade";
    }
}
