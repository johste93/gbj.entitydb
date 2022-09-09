using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GBJ.EntityDB
{
    [JsonConverter(typeof(AssetReferenceHolder))]
    public class AssetReferenceHolderConverter<T> : JsonConverter where T : AssetReferenceHolder, new()
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            AssetReferenceHolder assetReferenceHolder = (AssetReferenceHolder) value;

            if (assetReferenceHolder != null)
            {
                if (assetReferenceHolder.GetAssetReference() != null)
                {
                    if (!string.IsNullOrEmpty(assetReferenceHolder.GetAssetReference().AssetGUID))
                    {
                        JObject o = new JObject();

                        o.Add("AssetGUID", assetReferenceHolder.GetAssetReference().AssetGUID);
                        o.Add("SubObjectName", assetReferenceHolder.GetAssetReference().SubObjectName);

                        o.WriteTo(writer);
                        return;
                    }
                }
            }

            writer.WriteNull();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            JObject o = JObject.Load(reader);

            var result = new T();
            result.NewAssetReference(o["AssetGUID"].Value<string>());

            if (o.ContainsKey("SubObjectName"))
                result.GetAssetReference().SubObjectName = o["SubObjectName"].Value<string>();

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}