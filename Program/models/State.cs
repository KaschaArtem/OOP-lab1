using System.Text;

public class State {

    public List<Tweet> Tweets { get; private set; } = new();

    public State() { }

    public State(List<Tweet> tweets) {
        Tweets = tweets;
    }

    public double Weight {
        get {
            double totalWeight = 0.0;
            int count = Tweets.Count;
            if (count == 0)
                count = 1;
            foreach (var tweet in Tweets)
                totalWeight += tweet.Weight;
            return Math.Round(totalWeight / count, 3);
        }
    }

    public void AddTweet(Tweet tweet) {
        Tweets.Add(tweet);
    }

    public override string ToString() {
        var str = new StringBuilder();
        str.AppendLine(this.Weight.ToString());
        foreach (var tweet in Tweets)
            str.AppendLine(tweet.ToString());
        return str.ToString();
    }
}
