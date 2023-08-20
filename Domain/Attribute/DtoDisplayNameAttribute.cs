namespace Domain.Attribute;

/// <summary>
/// Swagger DisplaynameAttribute.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DtoDisplayNameAttribute : System.Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DtoDisplayNameAttribute"/> class.
    /// </summary>
    /// <param name="displayName">Displayname of dto.</param>
    public DtoDisplayNameAttribute(string displayName)
    {
        this.DisplayName = displayName;
    }

    /// <summary>
    /// Gets displayname for Swagger.
    /// </summary>
    /// <value>String displayname.</value>
    public string DisplayName { get; }
}