using System.Text.RegularExpressions;

public class TweetTextParser {

    private Dictionary<string, double> sentiments;

    public TweetTextParser(string sentimentsFile) {
        sentiments = ParseSentimentFile(sentimentsFile);
    }

    private Dictionary<string, double> ParseSentimentFile(string sentimentsFile) {
        var sentiments = new Dictionary<string, double>();

        foreach (var line in File.ReadLines(sentimentsFile)) {
            var parts = line.Split(',');

            string word = parts[0].Trim();
            string scoreText = parts[1].Trim();

            if (double.TryParse(scoreText, out double score))
                sentiments[word] = score;
        }

        return sentiments;
    }

    public double GetWeight(string text) {
        double totalWeight = 0.0;
        int count = 0;

        text = text.ToLowerInvariant();
        var tokens = Regex.Matches(text, @"\b[\w\-]+\b");

        for (int i = 0; i < tokens.Count; i++) {
            string one = tokens[i].Value;
            string two = String.Empty;
            string three = String.Empty;

            if (i + 1 < tokens.Count)
                two = one + " " + tokens[i + 1].Value;
            if (i + 2 < tokens.Count)
                three = one + " " + tokens[i + 1].Value + " " + tokens[i + 2].Value;

            if (three != String.Empty && sentiments.TryGetValue(three, out double weight3)) {
                totalWeight += weight3;
                count++;
                i += 2;
            }
            else if (two != String.Empty && sentiments.TryGetValue(two, out double weight2)) {
                totalWeight += weight2;
                count++;
                i += 1;
            }
            else if (sentiments.TryGetValue(one, out double weight1)) {
                totalWeight += weight1;
                count++;
            }
        }

        if (count == 0)
            count = 1;

        return totalWeight / count;
    }
}
