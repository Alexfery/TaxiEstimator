@page "/"
@using TaxiEstimator.Components
@using TaxiEstimator.Models
@using TaxiEstimator.Services
@inject FareService FareService
@inject HttpClient Http

<div class="page-container">
    <div class="map-wrapper">
        <MapComponent @ref="_mapRef" 
                      AccessToken="PK_TOKEN_AICI" 
                      OnMapClicked="HandleMapClick" />
    </div>

    <div class="controls-panel">
        <h3>Estimator Taxi</h3>
        
        <div class="status">
            @if (_startPoint == null)
            {
                <p>📍 Apasă pe hartă pentru <b>Pornire</b></p>
            }
            else if (_endPoint == null)
            {
                <p>🏁 Apasă pe hartă pentru <b>Destinație</b></p>
            }
            else
            {
                <button class="btn btn-primary" @onclick="CalculateRoute">🚗 Calculează Preț</button>
                <button class="btn btn-secondary" @onclick="Reset">Reset</button>
            }
        </div>

        @if (_estimate!= null)
        {
            <div class="result-card">
                <h4>Estimare: @_estimate.TotalPrice @_estimate.Currency</h4>
                <p>📏 Distanță: @_estimate.DistanceKm km</p>
                <p>⏱️ Durată: @_estimate.DurationMin min</p>
            </div>
        }
    </div>
</div>

<style>
   .page-container { display: flex; flex-direction: column; height: 100vh; }
   .map-wrapper { flex: 1; position: relative; }
   .controls-panel { 
        height: 250px; 
        background: white; 
        padding: 20px; 
        box-shadow: 0 -2px 10px rgba(0,0,0,0.1); 
        z-index: 10;
    }
   .result-card { 
        background: #e6f7ff; 
        padding: 10px; 
        border-radius: 8px; 
        margin-top: 10px;
        border: 1px solid #91d5ff;
    }
</style>

@code {
    private MapComponent _mapRef;
    private GeoCoordinates _startPoint;
    private GeoCoordinates _endPoint;
    private PriceEstimate _estimate;

    private async Task HandleMapClick(GeoCoordinates coords)
    {
        if (_startPoint == null)
        {
            _startPoint = coords;
            await _mapRef.SetMarkerAsync("start", coords.Lat, coords.Lng);
        }
        else if (_endPoint == null)
        {
            _endPoint = coords;
            await _mapRef.SetMarkerAsync("end", coords.Lat, coords.Lng);
        }
    }

    private async Task CalculateRoute()
    {
        if (_startPoint == null |

| _endPoint == null) return;

        // Construim URL-ul pentru Mapbox Directions API
        // ATENTIE: În producție, acest call trebuie făcut prin propriul backend pentru a ascunde token-ul!
        var token = "PK_TOKEN_AICI"; 
        var url = $"https://api.mapbox.com/directions/v5/mapbox/driving/{_startPoint.Lng},{_startPoint.Lat};{_endPoint.Lng},{_endPoint.Lat}?geometries=geojson&access_token={token}";

        try 
        {
            var response = await Http.GetFromJsonAsync<MapboxResponse>(url);
            
            if (response?.Routes!= null && response.Routes.Length > 0)
            {
                var routeData = response.Routes;
                
                // 1. Desenăm ruta pe hartă (Geometry este deja obiect, trebuie serializat ca string pentru metoda noastră)
                var geoJsonString = System.Text.Json.JsonSerializer.Serialize(routeData.Geometry);
                await _mapRef.DrawRouteAsync(geoJsonString);

                // 2. Calculăm prețul
                var routeInfo = new RouteInfo 
                { 
                    DistanceMeters = routeData.Distance, 
                    DurationSeconds = routeData.Duration 
                };
                
                _estimate = FareService.CalculateFare(routeInfo);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare API: {ex.Message}");
        }
    }

    private void Reset()
    {
        _startPoint = null;
        _endPoint = null;
        _estimate = null;
        // Aici ar trebui o metodă în JS pentru a curăța harta (ClearMap), lăsată ca exercițiu.
    }

    // Clase interne pentru deserializarea răspunsului Mapbox JSON
    public class MapboxResponse { public MapboxRoute Routes { get; set; } }
    public class MapboxRoute 
    { 
        public double Distance { get; set; } 
        public double Duration { get; set; } 
        public object Geometry { get; set; } // GeoJSON object
    }
}