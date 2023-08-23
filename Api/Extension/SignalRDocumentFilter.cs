using Interface.Hubs;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Extension;

/// <summary>
/// Register interface that defines signalR clientside methods.
/// </summary>
public class SignalRDocumentFilter : IDocumentFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        context.SchemaGenerator
            .GenerateSchema(typeof(ILobbyClient), context.SchemaRepository);

        context.SchemaGenerator
            .GenerateSchema(typeof(ILobbyServer), context.SchemaRepository);
    }
}
