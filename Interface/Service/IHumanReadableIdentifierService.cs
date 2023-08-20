namespace Interface.Service;

/// <summary>
/// Interface for human readable identifier generator service.
/// </summary>
public interface IHumanReadableIdentifierService
{
    /// <summary>
    /// Gets a unique human readable identifier string.
    /// </summary>
    /// <returns>Human readable identifier string.</returns>
    Task<string> GenerateUniqueIdentifier();
}