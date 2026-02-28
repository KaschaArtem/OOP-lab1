public class Tweet {

    public Coordinates Coords { get; private set; } = new();
    public DateTime Date { get; private set; } = new();
    public string Text { get; private set; } = String.Empty;

    public double Weight { get; private set; }

    public Tweet() {}

    public Tweet(Coordinates coords, DateTime date, string text) {
        Coords = coords;
        Date = date;
        Text = text;

        Weight = CountTweetWeight();
    }
    
    private double CountTweetWeight() {
        return 0.0;
    }

    public override string ToString() {
        return $"{Coords.ToString()} {Date.ToString()} {Text}";
    }
}
