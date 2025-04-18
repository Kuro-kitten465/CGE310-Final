using Kuro.Dialogue;
using Kuro.GameSystem;
using UnityEngine;
using UnityEngine.Playables;

public class Scene_10_Controller : MonoBehaviour
{
    [SerializeField] private FadingHandler fadingHandler;
    [SerializeField] private DialogueData dialogueDataEnter;
    [SerializeField] private PlayableDirector director;

    private void Start()
    {
        fadingHandler.OnFadeCompleted += OnFadeCompleted;
        fadingHandler.StartFade(2f, FadingHandler.FadingType.FadeOut);
    }

    public void PlayCutscene()
    {
        director.Play();
    }

    private void OnFadeCompleted()
    {
        DialogueManager.Instance.StartDialogue(dialogueDataEnter);
        fadingHandler.OnFadeCompleted -= OnFadeCompleted;
    }
}