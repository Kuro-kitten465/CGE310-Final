using UnityEngine;
using UnityEngine.Playables;

namespace Kuro.Dialogue
{
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
}
