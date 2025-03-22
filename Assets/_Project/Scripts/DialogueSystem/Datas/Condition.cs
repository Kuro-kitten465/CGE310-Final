using System;

namespace KuroNovel.Data
{
    [Serializable]
    public class Condition
    {
        public string Expression;
        public string TrueNodeID;
        public string FalseNodeID;
    }
}
