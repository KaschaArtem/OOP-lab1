class Program {
    static void Main(string[] args) {
        
        var parser = new TweetParser();
        var country = new Country(parser.GetStates());
        
    }
}
