using System.Net.Json;

namespace GlLib.Common.Api
{
    public class TraitModifier : IJsonSerializable
    {
        public enum Operation
        {
            Add,
            AddPercent,
            Mult
        }

        public Operation operation = Operation.Add;
        public Trait trait = Trait.Health;
        public string uuid = "";
        public float value;

        public TraitModifier()
        {
        }

        public TraitModifier(string _uuid, Trait _trait, float _value, Operation _operation)
        {
            uuid = _uuid;
            trait = _trait;
            value = _value;
            operation = _operation;
        }

        public JsonObject Serialize(string _objectName)
        {
            var jsonObj = new JsonObjectCollection(_objectName);
            jsonObj.Add(new JsonStringValue("id", uuid));
            jsonObj.Add(new JsonNumericValue("traitId", trait.id));
            jsonObj.Add(new JsonNumericValue("value", value));
            jsonObj.Add(new JsonNumericValue("operation", (int) operation));
            return jsonObj;
        }

        public void Deserialize(JsonObject _jsonObject)
        {
            if (_jsonObject is JsonObjectCollection collection)
            {
                uuid = ((JsonStringValue) collection[0]).Value;
                trait = Trait.traits[(int) ((JsonNumericValue) collection[1]).Value];
                value = (float) ((JsonNumericValue) collection[2]).Value;
                operation = (Operation) ((JsonNumericValue) collection[3]).Value;
            }
        }

        public string GetStandardName()
        {
            return "traitModifier";
        }

        public override bool Equals(object _obj)
        {
            if (_obj is TraitModifier tm)
                return uuid == tm.uuid && trait == tm.trait && value == tm.value && operation == tm.operation;

            return false;
        }

        protected bool Equals(TraitModifier _other)
        {
            return string.Equals(uuid, _other.uuid) && Equals(trait, _other.trait) && value.Equals(_other.value) &&
                   operation == _other.operation;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = uuid != null ? uuid.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (trait != null ? trait.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ value.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) operation;
                return hashCode;
            }
        }
    }
}