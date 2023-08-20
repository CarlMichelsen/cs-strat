namespace Domain.Dto;

/// <summary>
/// Generic service response dto.
/// </summary>
/// <typeparam name="T">Generic dto object.</typeparam>
public class ServiceResponse<T>
{
    /// <summary>
    /// Gets a value indicating whether the response is Ok.
    /// Response is Ok if Data is not null and there are no errors.
    /// </summary>
    /// <value>Boolean Ok value.</value>
    public bool Ok { get => this.Data is not null && this.Errors.Count == 0; }

    /// <summary>
    /// Gets or sets generic data property.
    /// </summary>
    /// <value>Generic data property.</value>
    public T? Data { get; set; }

    /// <summary>
    /// Gets list of errors in the response.
    /// The list is initialized and empty by default.
    /// </summary>
    /// <returns>List of error strings.</returns>
    public List<string> Errors { get; init; } = new List<string>();

    /// <summary>
    /// Helper method for returning an error.
    /// </summary>
    /// <param name="argErrors">Errors.</param>
    /// <returns>Error serviceresponse.</returns>
    public static ServiceResponse<T> Error(params string[] argErrors)
    {
        return new ServiceResponse<T>
        {
            Errors = argErrors.ToList(),
        };
    }

    /// <summary>
    /// Helper method for returning an error.
    /// </summary>
    /// <param name="data">Data in Ok response.</param>
    /// <returns>Ok serviceresponse.</returns>
    public static ServiceResponse<T> OkResponse(T data)
    {
        return new ServiceResponse<T>
        {
            Data = data,
        };
    }
}