using System;
using Kuro.Dialogue;
using Kuro.GameSystem;
using UnityEngine;

public class Scene_6_Controller : MonoBehaviour
{
    [SerializeField] private FadingHandler fadingHandler;
    [SerializeField] private DialogueData dialogueData;

    private void Start()
    {
        fadingHandler.OnFadeCompleted += OnFadeCompleted;
        fadingHandler.StartFade(2f, FadingHandler.FadingType.FadeOut);
    }

    public void OnEndedScene()
    {
        fadingHandler.StartFade(2f, FadingHandler.FadingType.FadeIn);
    }

    private void OnFadeCompleted()
    {
        DialogueManager.Instance.StartDialogue(dialogueData);
        fadingHandler.OnFadeCompleted -= OnFadeCompleted;
    }
}
