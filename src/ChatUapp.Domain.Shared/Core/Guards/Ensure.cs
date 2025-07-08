using System.Collections.Generic;

namespace ChatUapp.Core.Guards;

/// <summary>
/// Provides common validation guard clauses for business and application logic.
/// Throws <see cref="AppValidationException"/> when a rule is violated.
/// </summary>
public static class Ensure
{
    /// <summary>
    /// Throws if the given object is null.
    /// </summary>
    /// <typeparam name="T">Type of the object to check.</typeparam>
    /// <param name="obj">The object to validate.</param>
    /// <param name="name">The name of the parameter (used in the exception message).</param>
    public static void NotNull<T>(T? obj, string name)
    {
        if (obj is null)
        {
            throw new AppValidationException($"{name} must not be null.");
        }
    }

    /// <summary>
    /// Throws if the string is null, empty, or consists only of whitespace.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <param name="name">The name of the parameter (used in the exception message).</param>
    public static void NotNullOrEmpty(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new AppValidationException($"{name} must not be null or empty.");
        }
    }

    /// <summary>
    /// Throws if the integer value is less than or equal to the specified threshold.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="threshold">The minimum required value (exclusive).</param>
    /// <param name="name">The name of the parameter.</param>
    public static void GreaterThan(int value, int threshold, string name)
    {
        if (value <= threshold)
        {
            throw new AppValidationException($"{name} must be greater than {threshold}.");
        }
    }

    /// <summary>
    /// Throws if the value is not between the specified minimum and maximum (inclusive).
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <param name="name">The name of the parameter.</param>
    public static void InRange(int value, int min, int max, string name)
    {
        if (value < min || value > max)
        {
            throw new AppValidationException($"{name} must be between {min} and {max}.");
        }
    }

    /// <summary>
    /// Throws if the collection is null or has no elements.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="collection">The collection to validate.</param>
    /// <param name="name">The name of the parameter.</param>
    public static void NotEmpty<T>(ICollection<T>? collection, string name)
    {
        if (collection == null || collection.Count == 0)
        {
            throw new AppValidationException($"{name} must not be empty.");
        }
    }
}
