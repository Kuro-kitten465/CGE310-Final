using System;
using System.Collections.Generic;

namespace KuroNovel.Data
{
    [Serializable]
    public class Action
    {
        public string Function;
        public List<CommandParameter> Parameters = new List<CommandParameter>();
    }
}
