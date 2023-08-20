using Domain.Dto;
using Domain.Exception;
using Microsoft.Extensions.Logging;

namespace Businesslogic.Handler;

/// <summary>
/// Contains common methods useful in handlers.
/// </summary>
public abstract class BaseHandler
{
    /// <summary>
    /// Runs an action in a try/catch loop and handles uncaught exceptions nicely.
    /// </summary>
    /// <param name="logger">A logger to log errors with.</param>
    /// <param name="func">The action that is being taken.</param>
    /// <typeparam name="T">Return type.</typeparam>
    /// <returns>Return type in a serviceresponse.</returns>
    protected async Task<ServiceResponse<T>> MapToServiceResponse<T>(ILogger logger, Func<Task<T>> func)
    {
        try
        {
            var value = await func();
            return ServiceResponse<T>.OkResponse(value);
        }
        catch (RepositoryException e)
        {
            logger.LogCritical(e.Message, e);
            return ServiceResponse<T>.Error("Database error");
        }
        catch (ServiceException e)
        {
            logger.LogCritical(e.Message, e);
            return ServiceResponse<T>.Error("Service error");
        }
        catch (System.Exception e)
        {
            logger.LogCritical(e.Message, e);
            return ServiceResponse<T>.Error("Fatal error");
        }
    }

    /// <summary>
    /// Runs an action in a try/catch loop and handles uncaught exceptions nicely.
    /// This runs synchronously.
    /// </summary>
    /// <param name="logger">A logger to log errors with.</param>
    /// <param name="func">The action that is being taken.</param>
    /// <typeparam name="T">Return type.</typeparam>
    /// <returns>Return type in a serviceresponse.</returns>
    protected ServiceResponse<T> MapToServiceResponseSync<T>(ILogger logger, Func<T> func)
    {
        try
        {
            var value = func();
            return ServiceResponse<T>.OkResponse(value);
        }
        catch (RepositoryException e)
        {
            logger.LogCritical(e.Message, e);
            return ServiceResponse<T>.Error("Database error");
        }
        catch (ServiceException e)
        {
            logger.LogCritical(e.Message, e);
            return ServiceResponse<T>.Error("Service error");
        }
        catch (System.Exception e)
        {
            logger.LogCritical(e.Message, e);
            return ServiceResponse<T>.Error("Fatal error");
        }
    }
}