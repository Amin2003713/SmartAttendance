using System;

namespace Shifty.Common.Utilities;

/// <summary>
/// Provides extension methods for working with enums decorated with the <see cref="FlagsAttribute"/>.
/// </summary>
/// <remarks>
/// This class contains generic extension methods that allow you to add, remove, toggle, check, and
/// manipulate flags on enum types using bitwise operations. Each method converts the enum values to a
/// <see cref="System.Int64"/> for manipulation, then converts the result back to the original enum type.
/// </remarks>
public static class FlagExtensions
{
    /// <summary>
    /// Adds the specified flag(s) to the enum value using a bitwise OR operation.
    /// </summary>
    /// <typeparam name="T">An enum type decorated with the <see cref="FlagsAttribute"/>.</typeparam>
    /// <param name="value">The original enum value.</param>
    /// <param name="flag">The flag(s) to add.</param>
    /// <returns>A new enum value with the specified flag(s) added.</returns>
    /// <remarks>
    /// Internally, the method converts the enum values to a <see cref="System.Int64"/> in order to perform
    /// the bitwise OR operation. The resulting numeric value is then converted back to the enum type.
    /// </remarks>
    public static T AddFlag<T>(this T value, T flag) where T : Enum
    {
        // Convert enum values to long integers for bitwise operations
        long valueLong = Convert.ToInt64(value);
        long flagLong  = Convert.ToInt64(flag);
        // Combine flags using bitwise OR operator
        long result = valueLong | flagLong;
        // Convert the numeric result back to the enum type and return
        return (T)Enum.ToObject(typeof(T), result);
    }

    /// <summary>
    /// Removes the specified flag(s) from the enum value using a bitwise AND with the complement of the flag(s).
    /// </summary>
    /// <typeparam name="T">An enum type decorated with the <see cref="FlagsAttribute"/>.</typeparam>
    /// <param name="value">The original enum value.</param>
    /// <param name="flag">The flag(s) to remove.</param>
    /// <returns>A new enum value with the specified flag(s) removed.</returns>
    /// <remarks>
    /// The method calculates the bitwise complement of the flag(s) to be removed, then applies a bitwise AND
    /// to clear the bits corresponding to the flag(s) in the original value.
    /// </remarks>
    public static T RemoveFlag<T>(this T value, T flag) where T : Enum
    {
        long valueLong = Convert.ToInt64(value);
        long flagLong  = Convert.ToInt64(flag);
        // Remove flag(s) by AND-ing with the complement of flag(s)
        long result = valueLong & ~flagLong;
        return (T)Enum.ToObject(typeof(T), result);
    }

    /// <summary>
    /// Toggles the specified flag(s) on the enum value using a bitwise XOR operation.
    /// </summary>
    /// <typeparam name="T">An enum type decorated with the <see cref="FlagsAttribute"/>.</typeparam>
    /// <param name="value">The original enum value.</param>
    /// <param name="flag">The flag(s) to toggle.</param>
    /// <returns>A new enum value with the specified flag(s) toggled.</returns>
    /// <remarks>
    /// This method uses the XOR operator to flip the bits corresponding to the flag(s). If a bit is set, it will be
    /// cleared; if a bit is cleared, it will be set.
    /// </remarks>
    public static T ToggleFlag<T>(this T value, T flag) where T : Enum
    {
        long valueLong = Convert.ToInt64(value);
        long flagLong  = Convert.ToInt64(flag);
        // Toggle flag(s) using bitwise XOR operator
        long result = valueLong ^ flagLong;
        return (T)Enum.ToObject(typeof(T), result);
    }

    /// <summary>
    /// Checks whether the specified flag(s) are set in the enum value.
    /// </summary>
    /// <typeparam name="T">An enum type decorated with the <see cref="FlagsAttribute"/>.</typeparam>
    /// <param name="value">The enum value to check.</param>
    /// <param name="flag">The flag(s) to check for.</param>
    /// <returns>
    /// <c>true</c> if all specified flag(s) are set in the enum value; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method performs a bitwise AND between the enum value and the flag(s), then compares the result
    /// to the flag(s) to determine if they are all present. Although enums have a built-in <see cref="Enum.HasFlag"/>
    /// method, this implementation provides insight into the underlying operation.
    /// </remarks>
    public static bool HasFlag<T>(this T value, T flag) where T : Enum
    {
        long valueLong = Convert.ToInt64(value);
        long flagLong  = Convert.ToInt64(flag);
        // Check if flag(s) are set by comparing the bitwise AND result with the flag value
        return (valueLong & flagLong) == flagLong;
    }

    /// <summary>
    /// Returns the underlying numeric value of the enum as a <see cref="System.Int64"/>.
    /// </summary>
    /// <typeparam name="T">An enum type.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <returns>The numeric representation of the enum value.</returns>
    /// <remarks>
    /// This method is useful for debugging or logging purposes, where the exact numeric value of a flag-based enum
    /// might be needed.
    /// </remarks>
    public static long GetValue<T>(this T value) where T : Enum
    {
        return Convert.ToInt64(value);
    }

    /// <summary>
    /// Sets or unsets the specified flag(s) on the enum value based on the provided Boolean state.
    /// </summary>
    /// <typeparam name="T">An enum type decorated with the <see cref="FlagsAttribute"/>.</typeparam>
    /// <param name="value">The original enum value.</param>
    /// <param name="flag">The flag(s) to modify.</param>
    /// <param name="state">
    /// <c>true</c> to add the flag(s); <c>false</c> to remove the flag(s).
    /// </param>
    /// <returns>A new enum value with the flag(s) set or unset according to the specified state.</returns>
    /// <remarks>
    /// This convenience method internally calls <see cref="AddFlag{T}"/> if <paramref name="state"/> is <c>true</c>
    /// or <see cref="RemoveFlag{T}"/> if <paramref name="state"/> is <c>false</c>. This simplifies conditional
    /// flag manipulation in your code.
    /// </remarks>
    public static T SetFlag<T>(this T value, T flag, bool state) where T : Enum
    {
        return state ? value.AddFlag(flag) : value.RemoveFlag(flag);
    }
}