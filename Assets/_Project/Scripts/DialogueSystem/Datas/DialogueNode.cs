using System;
using System.Collections.Generic;
using UnityEngine;

namespace KuroNovel.Data
{
    [Serializable]
    public class DialogueNode
    {
        public string ID;
        public NodeType Type;
        public string NextNodeID;
        [TextArea(3, 10)]
        //public LocalizedString Content;
        public string CharacterID;
        public string Emotion;
        public List<Command> Commands = new List<Command>();
        public List<Choice> Choices = new List<Choice>();
        public Condition NodeCondition;
        public List<Action> Actions = new List<Action>();
    }
}
