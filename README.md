# 🚖 TaxiEstimator

A cross‑platform mobile application for estimating taxi fares using real‑time routing data. Built with **.NET MAUI Blazor Hybrid**, powered by **Mapbox GL JS** for mapping and **C#** for business logic.

---

## 📋 Features

- **Hybrid Architecture** — Native C# logic combined with a modern web UI (HTML/CSS/JS).  
- **Interactive Map** — Full Mapbox GL JS integration through a custom JavaScript interop layer.  
- **Route Visualization** — Draws accurate polylines between origin and destination.  
- **Fare Calculation** — Computes estimated taxi fares based on distance, duration, and base fare logic.  
- **Draggable Markers** *(Planned)* — Interactive start/end point selection.  
- **Cross‑Platform** — Runs on Android, iOS, Windows, and macOS.

---

## 🏗 Architecture Overview

This project follows a **Backend‑for‑Frontend (BFF)**‑style structure (simulated locally):

### **1. UI Layer**
Blazor components (`.razor`) rendered inside a `BlazorWebView`.

### **2. Interop Layer**
A JavaScript bridge (`mapboxInterop.js`) enabling C# to control the Mapbox map instance.

### **3. Service Layer**
- **MapboxService** — Handles HTTP requests to the Mapbox Directions API.  
- **FareService** — Pure C# implementation of the fare calculation logic.

---

## 🚀 Getting Started

### **Prerequisites**
- JetBrains Rider or Visual Studio  
- .NET SDK 8.0 or later  
- A **Mapbox account** with a public access token (`pk...`)  
- Android Emulator or a physical device for testing

---

## ⚙️ Configuration

### 1. Clone the Repository
```bash
git clone https://github.com/your-username/TaxiEstimator.git
cd TaxiEstimator
```

### 2. Set Your Mapbox Token
Open `Pages/Home.razor` and replace:

```csharp
var token = "PK_TOKEN_AICI";
```

with your actual **Mapbox Public Token**.

> ⚠️ **Security Note:**  
> Never hardcode API keys in production. Use a backend proxy to protect sensitive tokens.

### 3. Android Permissions
Ensure `Platforms/Android/AndroidManifest.xml` includes:

```xml
<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
<uses-permission android:name="android.permission.INTERNET" />
```

---

## 💻 Running in JetBrains Rider

1. Open `TaxiEstimator.sln` in Rider.  
2. Wait for NuGet packages to restore.  
3. Select the run configuration:  
   - Framework: `net8.0-android`  
   - Target device: e.g., *Pixel 5 – API 33*  
4. Press **Run** or **Debug**.

---

## 🧩 Project Structure

```
TaxiEstimator/
├── Components/
│   └── MapComponent.razor        # Map wrapper component
├── Models/
│   └── TripModels.cs             # Route and fare models
├── Pages/
│   └── Home.razor                # Main UI and logic
├── Services/
│   └── FareService.cs            # Fare calculation logic
├── wwwroot/
│   ├── js/
│   │   └── mapboxInterop.js      # JS bridge for Mapbox GL JS
│   └── index.html                # Web entry point
├── MauiProgram.cs                # Dependency Injection setup
└── _Imports.razor                # Global using directives
```

---

## 🧮 Fare Calculation Logic

The fare is computed using a standard taximeter formula:

```
Total = BaseFare + (DistanceKm * RatePerKm) + (DurationMin * RatePerMin)
```

**Default Rates:**
- **Base Fare:** 3.50 RON  
- **Per Km:** 2.50 RON  
- **Per Minute:** 0.45 RON  

---

## 🔧 Troubleshooting

- **Map not loading?**  
  Check internet access (emulator must have Wi‑Fi) and verify your Mapbox token.

- **"Cannot resolve symbol"?**  
  Ensure `_Imports.razor` includes the correct namespaces.

- **Android rendering issues?**  
  Mapbox uses WebGL — enable **Hardware Acceleration** in the Android Emulator settings.

---

## 🤝 Contributing

1. Fork the repository  
2. Create a feature branch  
3. Commit your changes  
4. Push the branch  
5. Open a Pull Request  


---

*Developed with ❤️ using .NET MAUI & JetBrains Rider*

---
