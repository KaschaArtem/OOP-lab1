using SkiaSharp;
using System.Text.Json;

public class StateDrawer {

    private const double MinLon = -179;
    private const double MaxLon = -66;
    private const double MinLat = 18;
    private const double MaxLat = 72;

    private Dictionary<USAStateName, List<List<Coordinates>>> polygons;

    public StateDrawer(string statesFile) {
        polygons = ParseStatesFile(statesFile);
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
            coords.Add(new Coordinates(lat, lon));
        }
        return coords;
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
            path.MoveTo(ScaleX(ring[0].Longitude), ScaleY(ring[0].Latitude));

            foreach (var p in ring) {
                sumLon += p.Longitude;
                sumLat += p.Latitude;
                count++;
            }

            for (int i = 1; i < ring.Count; i++)
                path.LineTo(ScaleX(ring[i].Longitude), ScaleY(ring[i].Latitude));

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
