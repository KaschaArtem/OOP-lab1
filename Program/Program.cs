class Program {
    static void Main(string[] args) {

        string GetDataPath() {
            string? dir = Directory.GetCurrentDirectory();

            while (!string.IsNullOrEmpty(dir)) {
                var slnFiles = Directory.GetFiles(dir, "*.sln")
                    .Concat(Directory.GetFiles(dir, "*.slnx"));
                if (slnFiles.Any())
                    return Path.Combine(dir, "data");

                dir = Directory.GetParent(dir)?.FullName;
            }

            throw new DirectoryNotFoundException("No solution file found.");
        }

        string GetTopicFile(string dataPath) {
            string topic = String.Empty;
            string[] allFiles = Directory.GetFiles(dataPath);
            List<string> tweetFileNames = new();

            foreach (var file in allFiles) {
                string fileName = Path.GetFileName(file);
                if (fileName.Contains("_tweets") && fileName.Contains(".txt"))
                    tweetFileNames.Add(fileName);
            }

            if (tweetFileNames.Count == 0) {
                Console.WriteLine("Nothing to parse.");
                return String.Empty;
            }

            Console.WriteLine("Choose file to parse:");
            for (int i = 1; i <= tweetFileNames.Count; i++)
                Console.WriteLine($"{i}) {tweetFileNames[i - 1]}");

            string? input = Console.ReadLine();
            try {
                int number = int.Parse(input!);
                topic = tweetFileNames[number - 1];
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return String.Empty;
            }

            return topic;
        }

        string dataPath = GetDataPath();

        var topic = GetTopicFile(dataPath);
        if (topic == String.Empty)
            return;

        var parser = new TweetParser(dataPath);
        var country = new Country(parser.GetStatesByTopic(topic));

        Console.WriteLine(country);
    }
}
