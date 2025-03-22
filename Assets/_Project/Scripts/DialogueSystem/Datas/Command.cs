using System;
using System.Collections.Generic;

namespace KuroNovel.Data
{
    [Serializable]
    public class Command
    {
        public CommandType Type;
        public List<CommandParameter> Parameters = new List<CommandParameter>();
    }

    [Serializable]
    public class CommandParameter
    {
        public string Name;
        public string StringValue;
        public float FloatValue;
        public bool BoolValue;
        public ParameterType _ParameterType;

        public enum ParameterType { String, Float, Bool }

        public object GetValue()
        {
            return _ParameterType switch
            {
                ParameterType.String => StringValue,
                ParameterType.Float => FloatValue,
                ParameterType.Bool => BoolValue,
                _ => null
            };
        }
    }
}
