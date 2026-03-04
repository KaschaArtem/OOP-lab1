using System.Text.RegularExpressions;

public class TweetTextParser {

    private Dictionary<string, double> sentiments;

    public TweetTextParser(string sentimentsFile) {
        sentiments = ParseSentimentsFile(sentimentsFile);
    }

    private Dictionary<string, double> ParseSentimentsFile(string sentimentsFile) {
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

    public double? GetWeight(string text) {
        const int MaxGramSize = 4;

        double totalWeight = 0.0;
        int count = 0;

        text = text.ToLowerInvariant();
        var tokens = Regex.Matches(
            Regex.Replace(text, @"[^\w\s]", ""),
            @"\b\w+\b"
        ).Select(match => match.Value).ToList();

        for (int i = 0; i < tokens.Count; i++)
            for (int size = MaxGramSize; size >= 1; size--) {
                if (i + size > tokens.Count)
                    continue;

                string phrase = string.Join(" ", tokens.Skip(i).Take(size));

                if (sentiments.TryGetValue(phrase, out double weight)) {
                    totalWeight += weight;
                    count++;
                    i += size - 1;
                    break;
                }
            }

        if (count == 0)
            return null;

        return Math.Round(totalWeight / count, 3);
    }
}
