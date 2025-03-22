using System.Collections.Generic;
using UnityEngine;

namespace KuroNovel.Data
{
    #region Enums
    public enum NodeType { Dialogue, Choice, Condition, Action }
    public enum CommandType { Show, Hide, Move, PlaySound, StopSound, PlayMusic, StopMusic, SetBackground, Wait, Effect }
    public enum VariableType { Boolean, Integer, Float, String }
    #endregion

    [CreateAssetMenu(fileName = "NovelScenario", menuName = "KuroNovel/Scenario")]
    public class NovelScenario : ScriptableObject
    {
        public string ID;
        public string Title;
        [TextArea(3, 10)]
        public string Description;
        public string StartNodeID;
        public List<DialogueNode> Nodes = new List<DialogueNode>();
        public List<DefaultVariable> Variables = new List<DefaultVariable>();
    }
}
