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

    public Dictionary<USAStateName, List<Tweet>> GetStatesByTopic(string topic) {
        Dictionary<USAStateName, List<Tweet>> states =
            Enum.GetValues<USAStateName>()
                .ToDictionary(state => state, state => new List<Tweet>());
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

            var tweetState = stateParser.GetState(tweetCoord);
            states[tweetState].Add(tweet);
        }

        return states;
    }
}
