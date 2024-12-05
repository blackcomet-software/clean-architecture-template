using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Abstractions;

namespace API.Serialization;

public class SvoToGuidJsonConverter<T> : JsonConverter<T>
    where T : struct, IGuidSvo
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetGuid();
        return (T)Activator.CreateInstance(typeof(T), value)!;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.ToString());
    }
}

public class SvoToIntJsonConverter<T> : JsonConverter<T>
    where T : struct, IIntSvo
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt32();
        return (T)Activator.CreateInstance(typeof(T), value)!;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}

public class SvoToStringJsonConverter<T> : JsonConverter<T>
    where T : struct, IStringSvo
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value is null) throw new JsonException();
        return (T)Activator.CreateInstance(typeof(T), value)!;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}