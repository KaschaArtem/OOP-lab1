public class Coordinates {

    public double Latitude { get; private set; }
    public double Longtitude { get; private set; }

    public Coordinates() {}

    public Coordinates(double latitude, double longtitude) {
        Latitude = latitude;
        Longtitude = longtitude;
    }

    public override string ToString() {
        return $"[{Latitude}, {Longtitude}]";
    }
}
