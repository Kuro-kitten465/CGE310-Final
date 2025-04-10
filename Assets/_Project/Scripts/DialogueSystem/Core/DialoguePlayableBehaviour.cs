using UnityEngine.Playables;

namespace Kuro.Dialogue
{
    public class DialoguePlayableBehaviour : PlayableBehaviour
    {
        public DialogueData dialogueData;
        private bool hasTriggeredDialogue = false;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (DialogueManager.Instance == null) return;
            
            if (!hasTriggeredDialogue && dialogueData != null)
            {
                DialogueManager.Instance.StartDialogue(dialogueData);
                hasTriggeredDialogue = true;
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            hasTriggeredDialogue = false;
        }
    }
}