using TaxiEstimator.Models;

namespace TaxiEstimator.Services;

public class FareService
{
    // Tarife standard (hardcodate pentru exemplu, pot veni din baza de date)
    private const decimal BaseFare = 3.50m; // Pornirea
    private const decimal PricePerKm = 2.50m;
    private const decimal PricePerMinute = 0.45m;

    public PriceEstimate CalculateFare(RouteInfo route)
    {
        var distKm = route.DistanceMeters / 1000.0;
        var durMin = route.DurationSeconds / 60.0;

        // Formula: Pornire + (Km * PretKm) + (Min * PretMin)
        // [1, 2]
        decimal total = BaseFare + 
                        ((decimal)distKm * PricePerKm) + 
                        ((decimal)durMin * PricePerMinute);

        // Rotunjire la 2 zecimale
        return new PriceEstimate
        {
            TotalPrice = Math.Round(total, 2),
            DistanceKm = Math.Round(distKm, 2),
            DurationMin = Math.Round(durMin, 2)
        };
    }
}