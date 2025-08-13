using ChatUapp.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ChatUapp.Core.Guards;

/// <summary>
/// Provides general-purpose guard clauses for enforcing business rules.
/// Typically used for throwing custom exceptions based on conditions.
/// </summary>
public static class AppGuard
{
    /// <summary>
    /// Throws <see cref="AppBusinessException"/> if the condition is true.
    /// </summary>
    /// <param name="condition">The condition that triggers the exception if true.</param>
    /// <param name="message">The message to include in the exception.</param>
    public static void Check([DoesNotReturnIf(true)] bool condition, string message)
    {
        if (condition)
        {
            throw new AppBusinessException(message);
        }
    }

    /// <summary>
    /// Throws the specified exception type if the condition is true.
    /// The exception type must have a constructor that accepts a single string argument.
    /// </summary>
    /// <typeparam name="TException">The exception type to throw.</typeparam>
    /// <param name="condition">The condition that triggers the exception if true.</param>
    /// <param name="message">The message to pass to the exception constructor.</param>
    public static void Check<TException>(
        [DoesNotReturnIf(true)] bool condition,
        string message)
        where TException : Exception, new()
    {
        if (condition)
        {
            var exception = (TException)Activator.CreateInstance(typeof(TException), message)!;
            throw exception;
        }
    }

    /// <summary>
    /// Throws the result of the provided exception factory if the condition is true.
    /// Allows for more complex exception construction logic.
    /// </summary>
    /// <typeparam name="TException">The exception type to throw.</typeparam>
    /// <param name="condition">The condition that triggers the exception if true.</param>
    /// <param name="exceptionFactory">A function that returns the exception to be thrown.</param>
    public static void Check<TException>(
        [DoesNotReturnIf(true)] bool condition,
        Func<TException> exceptionFactory)
        where TException : Exception
    {
        if (condition)
        {
            throw exceptionFactory();
        }
    }

    /// <summary>
    /// Throws <see cref="AppBusinessException"/> if permission is not granted.
    /// </summary>
    public static void HasPermission(bool hasPermission, string permissionName)
    {
        if (!hasPermission)
        {
            throw new AppBusinessException(
                $"You are not authorized to perform this action. Missing permission: {permissionName}"
            );
        }
    }
}
