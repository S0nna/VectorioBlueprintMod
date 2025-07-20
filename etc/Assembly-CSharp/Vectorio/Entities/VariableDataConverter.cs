using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Vectorio.Entities
{
	// Token: 0x0200029E RID: 670
	public class VariableDataConverter : JsonConverter
	{
		// Token: 0x06001301 RID: 4865 RVA: 0x0005749D File Offset: 0x0005569D
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(VariableData);
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x000574B0 File Offset: 0x000556B0
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jobject = JObject.Load(reader);
			string text = jobject["DataType"].Value<string>();
			if (text == "string")
			{
				return jobject.ToObject<StringVariableData>(serializer);
			}
			if (text == "int")
			{
				return jobject.ToObject<IntVariableData>(serializer);
			}
			if (text == "float")
			{
				return jobject.ToObject<FloatVariableData>(serializer);
			}
			if (!(text == "bool"))
			{
				throw new JsonSerializationException("Unsupported VariableData type: " + text);
			}
			return jobject.ToObject<BoolVariableData>(serializer);
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x00028A8B File Offset: 0x00026C8B
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
