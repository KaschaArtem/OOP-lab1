public class Tweet {

    public Coordinates Coords { get; private set; } = new();
    public string Date { get; private set; } = String.Empty;
    public string Time { get; private set; } = String.Empty;
    public string Text { get; private set; } = String.Empty;

    public double Weight { get; private set; }

    public Tweet() {}

    public Tweet(Coordinates coords, string date, string text) {
        Coords = coords;
        Date = date;
        Text = text;

        Weight = CountTweetWeight();
    }
    
    private double CountTweetWeight() {
        return 0.0;
    }

    public override string ToString() {
        return $"{Coords.ToString()} {Date} {Time} {Text}";
    }
}
