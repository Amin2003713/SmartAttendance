namespace App.Common.General.Enums.Dashboard;

public enum GrowthType : byte
{
    /// <summary>
    ///     The value has increased compared to the previous period.
    /// </summary>
    Expanded,

    /// <summary>
    ///     The value has decreased compared to the previous period.
    /// </summary>
    Shrink,

    /// <summary>
    ///     The value has remained the same compared to the previous period.
    /// </summary>
    Stable
}