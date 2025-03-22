using System;

namespace KuroNovel.Data
{
    [Serializable]
    public class Choice
    {
        //public LocalizedString Text;
        public string NextNodeID;
        public string Condition; // Expression to evaluate
    }
}
