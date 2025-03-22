using System;

namespace KuroNovel.Data
{
    [Serializable]
    public class DefaultVariable
    {
        public string Name;
        public VariableType Type;
        public string StringValue;
        public float FloatValue;
        public bool BoolValue;
        
        public object GetDefaultValue()
        {
            return Type switch
            {
                VariableType.String => StringValue,
                VariableType.Float => FloatValue,
                VariableType.Integer => (int)FloatValue,
                VariableType.Boolean => BoolValue,
                _ => null
            };
        }
    }
}
