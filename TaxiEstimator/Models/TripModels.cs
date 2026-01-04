namespace TaxiEstimator.Models;

public class RouteInfo
{
    public double DistanceMeters { get; set; }
    public double DurationSeconds { get; set; }
    public string GeometryGeoJson { get; set; } = string.Empty; // Linia rutei codată
}

public class PriceEstimate
{
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; } = "RON";
    public double DistanceKm { get; set; }
    public double DurationMin { get; set; }
}
public class TripModel
{
    // Coordonatele de plecare (longitudine, latitudine)
    public double StartLng { get; set; }
    public double StartLat { get; set; }

    // Coordonatele de destinație
    public double EndLng { get; set; }
    public double EndLat { get; set; }

    // Rezultatele calculate
    public double DistanceKm { get; set; }
    public double DurationMin { get; set; }
    public decimal EstimatedPrice { get; set; }
}
public class GeoCoordinates
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}