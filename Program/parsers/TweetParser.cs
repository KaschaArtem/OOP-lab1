using System.Globalization;

public class TweetParser {

    private string DataPath = String.Empty;

    private StateParser stateParser;
    private TweetTextParser tweetTextParser;

    public TweetParser(string dataPath) {
        DataPath = dataPath;

        stateParser = new StateParser(Path.Combine(dataPath, "states.json"));
        tweetTextParser = new TweetTextParser(Path.Combine(dataPath, "sentiments.csv"));
    }

    public Dictionary<USAStateName, State> GetStatesByTopic(string topic) {
        Dictionary<USAStateName, State> states =
            Enum.GetValues<USAStateName>()
                .ToDictionary(state => state, state => new State());
        string topicFile = Path.Combine(DataPath, topic);

        foreach (var line in File.ReadLines(topicFile)) {
            var parts = line.Split('\t');

            string coordPart = parts[0].Trim('[', ']');
            var coordValues = coordPart.Split(',');
            double latitude = double.Parse(coordValues[0]);
            double longitude = double.Parse(coordValues[1]);

            var tweetCoord = new Coordinates(latitude, longitude);
            DateTime dateTime = DateTime.ParseExact(parts[2], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            string text = parts[3];

            var tweet = new Tweet(tweetCoord, dateTime, text, tweetTextParser);

            var tweetStateName = stateParser.GetStateName(tweetCoord);
            states[tweetStateName].AddTweet(tweet);
        }

        return states;
    }
}
