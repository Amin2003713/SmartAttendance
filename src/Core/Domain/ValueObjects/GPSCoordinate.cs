using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.ValueObjects;

// VO: مختصات GPS با محدوده معتبر
public sealed class GPSCoordinate
{
	public double Latitude { get; }
	public double Longitude { get; }

	public GPSCoordinate(double latitude, double longitude)
	{
		if (latitude is < -90 or > 90) throw new DomainValidationException("عرض جغرافیایی نامعتبر است.");
		if (longitude is < -180 or > 180) throw new DomainValidationException("طول جغرافیایی نامعتبر است.");
		Latitude = latitude;
		Longitude = longitude;
	}

	public double DistanceTo(GPSCoordinate other)
	{
		const double R = 6371000;
		double dLat = ToRad(other.Latitude - Latitude);
		double dLon = ToRad(other.Longitude - Longitude);
		double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
		           Math.Cos(ToRad(Latitude)) * Math.Cos(ToRad(other.Latitude)) *
		           Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
		double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
		return R * c;
	}

	private static double ToRad(double d) => d * Math.PI / 180.0;
}

