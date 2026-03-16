using SkiaSharp;
using System.Text.Json;

public class StateDrawer {

    private double MinLon;
    private double MaxLon;
    private double MinLat;
    private double MaxLat;

    private Dictionary<USAStateName, List<List<Coordinates>>> polygons;

    public StateDrawer(string statesFile) {
        polygons = ParseStatesFile(statesFile);
        CalculateBounds();
    }

    private Dictionary<USAStateName, List<List<Coordinates>>> ParseStatesFile(string statesFile) {
        var polygons = new Dictionary<USAStateName, List<List<Coordinates>>>();

        var json = File.ReadAllText(statesFile);
        var doc = JsonDocument.Parse(json);

        foreach (var stateProp in doc.RootElement.EnumerateObject()) {
            if (!Enum.TryParse<USAStateName>(stateProp.Name, out var state))
                continue;

            var statePolygons = new List<List<Coordinates>>();
            foreach (var polygonElement in stateProp.Value.EnumerateArray())
                if (polygonElement[0][0].ValueKind == JsonValueKind.Number) {
                    var polygon = ParseRing(polygonElement);
                    statePolygons.Add(polygon);
                }
                else
                    foreach (var ring in polygonElement.EnumerateArray()) {
                        var polygon = ParseRing(ring);
                        statePolygons.Add(polygon);
                    }

            polygons[state] = statePolygons;
        }

        return polygons;
    }

    private List<Coordinates> ParseRing(JsonElement ring) {
        var coords = new List<Coordinates>();
        foreach (var point in ring.EnumerateArray()) {
            double lon = point[0].GetDouble();
            double lat = point[1].GetDouble();
            if (lon > 0)
                lon -= 360;
            coords.Add(new Coordinates(lat, lon));
        }
        return coords;
    }

    private void CalculateBounds() {
        MinLon = double.MaxValue;
        MaxLon = double.MinValue;
        MinLat = double.MaxValue;
        MaxLat = double.MinValue;


        foreach (var kvp in polygons) {
            var state = kvp.Key;

            foreach (var ring in kvp.Value) {
                foreach (var p in ring) {
                    var (lon, lat) = Transform(state, p.Longitude, p.Latitude);
                    if (lon < MinLon) MinLon = lon;
                    if (lon > MaxLon) MaxLon = lon;
                    if (lat < MinLat) MinLat = lat;
                    if (lat > MaxLat) MaxLat = lat;
                }
            }
        }

        double padding = 0.02;

        double lonRange = MaxLon - MinLon;
        double latRange = MaxLat - MinLat;

        MinLon -= lonRange * padding;
        MaxLon += lonRange * padding;
        MinLat -= latRange * padding;
        MaxLat += latRange * padding;
    }

    private (double lon, double lat) Transform(USAStateName state, double lon, double lat) {
        if (state == USAStateName.AK) {
            lon = lon * 0.3 - 75;
            lat = lat * 0.4 + 2;
        }
        else if (state == USAStateName.HI) {
            lon += 50;
            lat += 6;
        }
        return (lon, lat);
    }

    public void DrawState(SKColor color, USAStateName state, SKCanvas canvas, int width, int height)
    {
        float ScaleX(double lon) =>
            (float)((lon - MinLon) / (MaxLon - MinLon) * width);

        float ScaleY(double lat) =>
            (float)((MaxLat - lat) / (MaxLat - MinLat) * height);

        using var fillPaint = new SKPaint {
            Style = SKPaintStyle.Fill,
            Color = color,
        };

        using var borderPaint = new SKPaint {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 1,
        };


        using var textPaint = new SKPaint {
            Color = SKColors.Black,
        };

        using var font = new SKFont {
            Size = 26
        };

        using var outlinePaint = new SKPaint {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 3,
            Color = SKColors.White,
        };

        double sumLon = 0;
        double sumLat = 0;
        int count = 0;

        foreach (var ring in polygons[state]) {
            if (ring.Count == 0)
                continue;

            var path = new SKPath();
            var (lon0, lat0) = Transform(state, ring[0].Longitude, ring[0].Latitude);
            path.MoveTo(ScaleX(lon0), ScaleY(lat0));

            foreach (var p in ring) {
                var (lon, lat) = Transform(state, p.Longitude, p.Latitude);
                sumLon += lon;
                sumLat += lat;
                count++;
            }

            for (int i = 1; i < ring.Count; i++) {
                var (lon, lat) = Transform(state, ring[i].Longitude, ring[i].Latitude);
                path.LineTo(ScaleX(lon), ScaleY(lat));
            }

            path.Close();
            canvas.DrawPath(path, fillPaint);
            canvas.DrawPath(path, borderPaint);
        }

        if (count > 0) {
            float x = ScaleX(sumLon / count);
            float y = ScaleY(sumLat / count);

            string label = state.ToString();

            var bounds = new SKRect();
            font.MeasureText(label, out bounds);

            float textX = x - bounds.MidX;
            float textY = y - bounds.MidY;

            canvas.DrawText(label, textX, textY, font, outlinePaint);
            canvas.DrawText(label, textX, textY, font, textPaint);
        }
    }
}
