public class Tweet {

    public Coordinates Coords { get; private set; } = new();
    public DateTime Date { get; private set; } = new();
    public string Text { get; private set; } = String.Empty;

    public double Weight { get; private set; }

    public Tweet() {}

    public Tweet(Coordinates coords, DateTime date, string text, TweetTextParser tweetTextParser) {
        Coords = coords;
        Date = date;
        Text = text;

        Weight = CountTweetWeight(tweetTextParser);
    }

    private double CountTweetWeight(TweetTextParser tweetTextParser) {
        return tweetTextParser.GetWeight(Text);
    }

    public override string ToString() {
        return $"{Coords.ToString()} {Date.ToString()} {Text} ({Weight})";
    }
}
