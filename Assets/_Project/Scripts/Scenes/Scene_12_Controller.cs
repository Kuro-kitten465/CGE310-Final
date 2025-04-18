using Kuro.Dialogue;
using Kuro.GameSystem;
using Kuro.Utilities.DesignPattern;
using UnityEngine;
using UnityEngine.Playables;

public class Scene_12_Controller : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private DialogueData onLoseDialogueData;
    [SerializeField] private DialogueData onWinDialogueData;
    [SerializeField] private PlayableDirector playableDirector;

    private bool isMinigameFinish = false;

    private bool canPlayCutscene = false;

    public void LoadMiniGameState()
    {
        if (!isMinigameFinish)
        {
            EventBus.Publish(EventCollector.StopEvent);
            NumberPuzzleLoader.LoadPuzzle(OnPuzzleCompleted);
            return;
        }

        if (canPlayCutscene)
        {
            playableDirector.Play();
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
    }

    public void OnPuzzleCompleted()
    {
        isMinigameFinish = true;

        if (GameManager.Instance.GetFlag("WinPuzzle", out object winPuzzle) == GameManager.FlagType.BOOL && (bool)winPuzzle)
        {
            canPlayCutscene = true;
            DialogueManager.Instance.StartDialogue(onWinDialogueData);
        }
        else
        {
            canPlayCutscene = true;
            DialogueManager.Instance.StartDialogue(onLoseDialogueData);
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector obj)
    {
        playableDirector.stopped -= OnPlayableDirectorStopped;
        sceneLoader.LoadScene();
    }
}
