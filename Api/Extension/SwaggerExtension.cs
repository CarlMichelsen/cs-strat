using System.Reflection;
using Domain.Attribute;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Extension;

/// <summary>
/// Extensions for configuring swagger.
/// </summary>
internal static class SwaggerExtension
{
    /// <summary>
    /// Add swaggerGen with xml documentation from assembly.
    /// This is a lot of messy code that should not clutter Program.cs file.
    /// </summary>
    /// <param name="services">ServiceCollection to register to.</param>
    /// <param name="optionsAction">Options delegate function that runs before the xml docs have been added to swagger.</param>
    /// <returns>ServiceCollection.</returns>
    internal static IServiceCollection AddSwaggerGenWithXmlDocumentation(
        this IServiceCollection services,
        Action<SwaggerGenOptions> optionsAction)
    {
        return services.AddSwaggerGen((options) =>
        {
            optionsAction(options); // run additional options first

            var currentAssembly = Assembly.GetExecutingAssembly();
            var allAssemblies = new List<AssemblyName>(currentAssembly.GetReferencedAssemblies())
            {
                currentAssembly.GetName(),
            };

            foreach (var assembly in allAssemblies)
            {
                string xmlFile = $"{assembly.Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            }
        });
    }

    /// <summary>
    /// Consumes <see cref="DtoDisplayNameAttribute"/> and replaces swagger type-names with DtoDisplayName recursively.
    /// </summary>
    /// <param name="type">The type to be translated to a DtoDisplayName.</param>
    /// <returns>Type DtoDisplayName string.</returns>
    internal static string GetSchemaIdRecursively(Type type)
    {
        var attribute = (DtoDisplayNameAttribute?)type
            .GetCustomAttributes(typeof(DtoDisplayNameAttribute), false)
            .FirstOrDefault();

        if (attribute != null)
        {
            return attribute.DisplayName;
        }

        if (type.IsGenericType)
        {
            var typeName = type.GetGenericTypeDefinition().Name;

            // Remove `1 at the end of string if it exists
            typeName = typeName.Contains('`') ? typeName.Split('`')[0] : typeName;

            var typeArguments = type.GetGenericArguments()
                .Select(GetSchemaIdRecursively) // Recursive call to handle generic type arguments
                .ToArray();

            return $"{typeName}<{string.Join(",", typeArguments)}>";
        }

        return type.Name;
    }
}