using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Unclutter.SDK.Models;
using Unclutter.Services.Games;

namespace Unclutter.Services.Converters
{
    public class GameCategoryConverter : JsonConverter<IGameCategory>
    {
        public override IGameCategory Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var category = new GameCategory();

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return category;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "category_id":
                            category.Id = reader.GetInt32();
                            break;
                        case "name":
                            category.Name = reader.GetString();
                            break;
                        case "parent_category":
                            if (reader.TokenType == JsonTokenType.Number)
                            {
                                category.ParentCategoryId = reader.GetInt32();
                            }
                            else
                            {
                                category.ParentCategoryId = -1;
                            }
                            break;
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, IGameCategory value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case null:
                    JsonSerializer.Serialize(writer, (IGameCategory)null, options);
                    break;
                default:
                    {
                        var type = value.GetType();
                        JsonSerializer.Serialize(writer, value, type, options);
                        break;
                    }
            }
        }
    }
}
