using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.ValueObjects;

// VO: مختصات GPS با محدوده معتبر
public sealed class GPSCoordinate
{
    public GPSCoordinate(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90) throw new DomainValidationException("عرض جغرافیایی نامعتبر است.");
        if (longitude is < -180 or > 180) throw new DomainValidationException("طول جغرافیایی نامعتبر است.");

        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; }
    public double Longitude { get; }

    public double DistanceTo(GPSCoordinate other)
    {
        const double R    = 6371000;
        var          dLat = ToRad(other.Latitude - Latitude);
        var          dLon = ToRad(other.Longitude - Longitude);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRad(Latitude)) *
                Math.Cos(ToRad(other.Latitude)) *
                Math.Sin(dLon / 2) *
                Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double ToRad(double d)
    {
        return d * Math.PI / 180.0;
    }
}