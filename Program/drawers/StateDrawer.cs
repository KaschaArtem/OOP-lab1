using System.Text.Json;
using System.Drawing;

public class StateDrawer {

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

    public void DrawPolygon(Color color, USAStateName state) {

    }
}
