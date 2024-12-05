using System.Reflection;
using Domain.Abstractions;
using Domain.Attributes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Configuration;

public static class SwaggerGenOptionsExtensions
{
    public static void ConfigureSwaggerGeneration(this SwaggerGenOptions options)
    {
        options.SupportNonNullableReferenceTypes();
        options.UseOpenApiDataTypeAttributes(typeof(OpenApiDataTypeAttribute).Assembly);
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                []
            }
        });
    }

    private static void UseOpenApiDataTypeAttributes(this SwaggerGenOptions options, Assembly assembly)
    {
        var svoTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => SvoInterfaces.Types.Contains(i)))
            .ToList();

        foreach (var type in svoTypes)
        {
            var attribute = type.GetCustomAttribute<OpenApiDataTypeAttribute>();
            if (attribute is not null)
            {
                options.MapType(type, () =>
                    new OpenApiSchema
                    {
                        Type = attribute.Type,
                        Example = OpenApiAnyFactory.CreateFromJson(attribute.Example.ToString()),
                        Format = attribute.Format,
                        Pattern = attribute.Pattern,
                        Nullable = attribute.Nullable,
                    });
            }
            else
            {
                options.MapType(type, () => type.GetSvoOpenApiSchema());
            }
        }
    }

    private static OpenApiSchema GetSvoOpenApiSchema(this Type type) =>
        type switch
        {
            _ when typeof(IStringSvo).IsAssignableFrom(type) => new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("Hello I'm a string"),
                Description = "A string single value object",
            },
            _ when typeof(IGuidSvo).IsAssignableFrom(type) => new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("343bded8-e63d-499c-9244-6e61174e94ff"),
                Description = "A Guid single value object",
                Format = "uuid",
            },
            _ when typeof(IIntSvo).IsAssignableFrom(type) => new OpenApiSchema
            {
                Type = "integer",
                Example = new OpenApiInteger(300),
                Description = "An int single value object",
                Format = "int32",
            },
            _ => throw new Exception("Unsupported svo type for swagger generation.")
        };
}