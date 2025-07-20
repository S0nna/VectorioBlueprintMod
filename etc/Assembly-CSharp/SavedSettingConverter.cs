using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vectorio.Settings;

// Token: 0x02000195 RID: 405
public class SavedSettingConverter : JsonConverter
{
	// Token: 0x06000D7D RID: 3453 RVA: 0x0003B218 File Offset: 0x00039418
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(BaseSetting);
	}

	// Token: 0x06000D7E RID: 3454 RVA: 0x0003B22C File Offset: 0x0003942C
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject jobject = JObject.Load(reader);
		string text = jobject["Type"].Value<string>();
		BaseSetting baseSetting;
		if (!(text == "Flag"))
		{
			if (!(text == "Vector"))
			{
				if (!(text == "Float"))
				{
					if (!(text == "Int"))
					{
						throw new JsonSerializationException("Unknown setting type: " + text);
					}
					baseSetting = new IntSetting();
				}
				else
				{
					baseSetting = new FloatSetting();
				}
			}
			else
			{
				baseSetting = new VectorSetting();
			}
		}
		else
		{
			baseSetting = new FlagSetting();
		}
		serializer.Populate(jobject.CreateReader(), baseSetting);
		return baseSetting;
	}

	// Token: 0x06000D7F RID: 3455 RVA: 0x0003B2C8 File Offset: 0x000394C8
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		BaseSetting baseSetting = (BaseSetting)value;
		writer.WriteStartObject();
		writer.WritePropertyName("Type");
		writer.WriteValue(baseSetting.Type);
		string type = baseSetting.Type;
		if (!(type == "Flag"))
		{
			if (!(type == "Vector"))
			{
				if (!(type == "Float"))
				{
					if (type == "Int")
					{
						IntSetting intSetting = (IntSetting)baseSetting;
						writer.WritePropertyName("Value");
						writer.WriteValue(intSetting.Value);
					}
				}
				else
				{
					FloatSetting floatSetting = (FloatSetting)baseSetting;
					writer.WritePropertyName("Value");
					writer.WriteValue(floatSetting.Value);
				}
			}
			else
			{
				VectorSetting vectorSetting = (VectorSetting)baseSetting;
				writer.WritePropertyName("ValueX");
				writer.WriteValue(vectorSetting.ValueX);
				writer.WritePropertyName("ValueY");
				writer.WriteValue(vectorSetting.ValueY);
			}
		}
		else
		{
			FlagSetting flagSetting = (FlagSetting)baseSetting;
			writer.WritePropertyName("Value");
			writer.WriteValue(flagSetting.Value);
		}
		writer.WriteEndObject();
	}
}
