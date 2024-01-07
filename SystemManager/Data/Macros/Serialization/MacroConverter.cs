using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManager.Data.Macros.DataModels;

namespace SystemManager.Data.Macros.Serialization
{
    public class MacroConverter : JsonConverter
    {

        //  METHODS

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(MacroBase));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            int macroTypeIndex = jsonObject.Value<int?>("MacroType") ?? -1;

            if (macroTypeIndex < 0)
                throw new NotSupportedException($"Unsupported MacroType {macroTypeIndex}");

            MacroType macroType = (MacroType)macroTypeIndex;
            MacroBase macroObject;

            switch (macroType)
            {
                case MacroType.Delay:
                    macroObject = new MacroDelay();
                    break;
                case MacroType.KeyDown:
                    macroObject = new MacroKeyDown();
                    break;
                case MacroType.KeyUp:
                    macroObject = new MacroKeyUp();
                    break;
                case MacroType.KeyClick:
                    macroObject = new MacroKeyClick();
                    break;
                case MacroType.KeyCombination:
                    macroObject = new MacroKeyCombination();
                    break;
                case MacroType.MouseDown:
                    macroObject = new MacroMouseDown();
                    break;
                case MacroType.MouseUp:
                    macroObject = new MacroMouseUp();
                    break;
                case MacroType.MouseClick:
                    macroObject = new MacroMouseClick();
                    break;
                case MacroType.MouseMove:
                    macroObject = new MacroMouseMove();
                    break;
                case MacroType.MouseScrollHorizontal:
                    macroObject = new MacroMouseScrollHorizontal();
                    break;
                case MacroType.MouseScrollVertical:
                    macroObject = new MacroMouseScrollVertical();
                    break;
                default:
                    throw new NotSupportedException($"Unsupported MacroType");
            }

            serializer.Populate(jsonObject.CreateReader(), macroObject);

            return macroObject;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

    }
}
