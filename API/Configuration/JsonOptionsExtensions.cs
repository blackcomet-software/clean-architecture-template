using System.Reflection;
using System.Text.Json.Serialization;
using API.Serialization;
using Domain.Abstractions;
using Domain.Models.Users;
using Microsoft.AspNetCore.Http.Json;

namespace API.Configuration;

public static class JsonOptionsExtensions
{
    public static void ConfigureJsonOptions(this JsonOptions jsonOptions)
    {
        jsonOptions.SerializerOptions.Converters.AddExplicitConverters();
        jsonOptions.SerializerOptions.Converters.AddSvoConverters();
    }

    private static void AddExplicitConverters(this IList<JsonConverter> converters)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var convertersTypesInAssembly = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType && t.IsSubclassOf(typeof(JsonConverter)))
            .ToList();

        foreach (var converterType in convertersTypesInAssembly)
        {
            var converter = (JsonConverter)Activator.CreateInstance(converterType)!;
            converters.Add(converter);
        }
    }

    private static void AddSvoConverters(this IList<JsonConverter> converters)
    {
        var domain = Assembly.GetAssembly(typeof(UserId));

        var svoTypes = domain!.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => SvoInterfaces.Types.Contains(i)))
            .ToList();

        foreach (var type in svoTypes)
        {
            var converterType = type switch
            {
                _ when typeof(IStringSvo).IsAssignableFrom(type) =>
                    typeof(SvoToStringJsonConverter<>).MakeGenericType(type),
                _ when typeof(IGuidSvo).IsAssignableFrom(type) =>
                    typeof(SvoToGuidJsonConverter<>).MakeGenericType(type),
                _ when typeof(IIntSvo).IsAssignableFrom(type) =>
                    typeof(SvoToIntJsonConverter<>).MakeGenericType(type),
                _ => throw new Exception("No JsonConverter found for svo type."),
            };
            
            var converter = (JsonConverter)Activator.CreateInstance(converterType)!;
            converters.Add(converter);
        }
    }
}