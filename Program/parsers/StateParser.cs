using System.Text.Json;

public class StateParser {

    private const double EarthRadiusKm = 6371.0;

    private Dictionary<USAStateName, Coordinates> stateCenters;

    public StateParser(string statesFile) {
        stateCenters = ParseStatesFile(statesFile);
    }

    private Dictionary<USAStateName, Coordinates> ParseStatesFile(string statesFile) {
        var stateCenters = new Dictionary<USAStateName, Coordinates>();

        var json = File.ReadAllText(statesFile);
        var doc = JsonDocument.Parse(json);

        foreach (var stateProp in doc.RootElement.EnumerateObject()) {
            string stateName = stateProp.Name;
            if (!Enum.TryParse<USAStateName>(stateName, out var state))
                continue;

            double latSum = 0, lonSum = 0;
            int count = 0;

            foreach (var polygon in stateProp.Value.EnumerateArray()) {
                foreach (var ringOrPoint in polygon.EnumerateArray()) {
                    if (ringOrPoint[0].ValueKind == JsonValueKind.Number) {
                        foreach (var point in polygon.EnumerateArray()) {
                            double lon = point[0].GetDouble();
                            double lat = point[1].GetDouble();
                            lonSum += lon;
                            latSum += lat;
                            count++;
                        }
                        break;
                    }
                    else {
                        foreach (var ring in polygon.EnumerateArray()) {
                            foreach (var point in ring.EnumerateArray())
                            {
                                double lon = point[0].GetDouble();
                                double lat = point[1].GetDouble();
                                lonSum += lon;
                                latSum += lat;
                                count++;
                            }
                        }
                    }
                }
            }

            if (count > 0)
                stateCenters[state] = new Coordinates(latSum / count, lonSum / count);
        }

        return stateCenters;
    }

    private double HaversineDistance(double lat1, double lon1, double lat2, double lon2) {
        double dLat = DegreesToRadians(lat2 - lat1);
        double dLon = DegreesToRadians(lon2 - lon1);

        lat1 = DegreesToRadians(lat1);
        lat2 = DegreesToRadians(lat2);

        double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Pow(Math.Sin(dLon / 2), 2);

        double c = 2 * Math.Asin(Math.Sqrt(a));

        return EarthRadiusKm * c;
    }

    private double DegreesToRadians(double degrees) {
        return degrees * Math.PI / 180.0;
    }

    public USAStateName GetState(Coordinates tweetCoordinates) {
        USAStateName closestState = default;

        double minDistance = double.MaxValue;
        foreach (var kvp in stateCenters) {
            double distance = HaversineDistance(
                tweetCoordinates.Latitude,
                tweetCoordinates.Longitude,
                kvp.Value.Latitude,
                kvp.Value.Longitude);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestState = kvp.Key;
            }
        }

        return closestState;
    }
}
