using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Kuro.Dialogue
{
    [TrackClipType(typeof(DialogueClip))]
    public class DialogueTrack : TrackAsset
    {
    }

    // Playable clip for timeline
    public class DialogueClip : PlayableAsset
    {
        public DialogueData dialogueData;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<DialoguePlayableBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();
            behaviour.dialogueData = dialogueData;
            return playable;
        }
    }

    // Playable behavior for timeline
    public class DialoguePlayableBehaviour : PlayableBehaviour
    {
        public DialogueData dialogueData;
        private bool hasTriggeredDialogue = false;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
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
