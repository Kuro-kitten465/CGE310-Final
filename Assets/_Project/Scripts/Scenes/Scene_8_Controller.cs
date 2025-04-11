using Kuro.Dialogue;
using Kuro.GameSystem;
using UnityEngine;
using UnityEngine.Playables;

public class Scene_8_Controller : MonoBehaviour
{
    [SerializeField] private FadingHandler fadingHandler;
    [SerializeField] private DialogueData dialogueDataEnter;
    [SerializeField] private PlayableDirector director;

    private void Start()
    {
        fadingHandler.OnFadeCompleted += OnFadeCompleted;
        fadingHandler.StartFade(2f, FadingHandler.FadingType.FadeOut);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>()._canMove = false;
            collision.GetComponent<Rigidbody2D>().linearVelocityX = 0f;
            director.Play();
        }
    }

    private void OnFadeCompleted()
    {
        DialogueManager.Instance.StartDialogue(dialogueDataEnter);
        fadingHandler.OnFadeCompleted -= OnFadeCompleted;
    }
}