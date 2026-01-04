// Variabilă globală pentru a ține instanța hărții
let map;
let startMarker;
let endMarker;

// Funcția de inițializare apelată din C#
export function initMap(containerId, accessToken, dotNetHelper) {
    mapboxgl.accessToken = accessToken;

    map = new mapboxgl.Map({
        container: containerId,
        style: 'mapbox://styles/mapbox/streets-v12',
        center: [26.1025, 44.4268], // București
        zoom: 12
    });

    // Adăugăm controale de navigare
    map.addControl(new mapboxgl.NavigationControl());

    // Ascultăm click-urile pe hartă pentru a pune pini
    map.on('click', (e) => {
        // Trimitem coordonatele înapoi în C#
        dotNetHelper.invokeMethodAsync('OnMapClick', e.lngLat.lat, e.lngLat.lng);
    });

    // Configurare layer pentru rută (gol inițial)
    map.on('load', () => {
        map.addSource('route', {
            'type': 'geojson',
            'data': {
                'type': 'Feature',
                'properties': {},
                'geometry': {
                    'type': 'LineString',
                    'coordinates':
                }
            }
        });

        map.addLayer({
            'id': 'route',
            'type': 'line',
            'source': 'route',
            'layout': {
                'line-join': 'round',
                'line-cap': 'round'
            },
            'paint': {
                'line-color': '#0080ff', // Culoare albastră
                'line-width': 6
            }
        });
    });
}

// Funcție pentru a desena markerii (Start/End)
export function setMarker(type, lat, lng) {
    const coords = [lng, lat];

    if (type === 'start') {
        if (startMarker) startMarker.remove();
        startMarker = new mapboxgl.Marker({ color: "#3bb2d0" }) // Cyan pentru Start
            .setLngLat(coords)
            .addTo(map);
    } else {
        if (endMarker) endMarker.remove();
        endMarker = new mapboxgl.Marker({ color: "#fbb03b" }) // Portocaliu pentru End
            .setLngLat(coords)
            .addTo(map);
    }
}

// Funcție pentru a desena linia rutei primită de la API
export function drawRoute(geoJsonGeometry) {
    if (map.getSource('route')) {
        map.getSource('route').setData(geoJsonGeometry);

        // Facem zoom să cuprindem toată ruta
        const coordinates = geoJsonGeometry.coordinates;
        const bounds = coordinates.reduce((bounds, coord) => {
            return bounds.extend(coord);
        }, new mapboxgl.LngLatBounds(coordinates, coordinates));

        map.fitBounds(bounds, { padding: 50 });
    }
}