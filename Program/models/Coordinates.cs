public class Coordinates {

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Coordinates() {}

    public Coordinates(double latitude, double longtitude) {
        Latitude = latitude;
        Longitude = longtitude;
    }

    public override string ToString() {
        return $"[{Latitude}, {Longitude}]";
    }
}
