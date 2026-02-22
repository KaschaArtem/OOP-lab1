using System.Text;

public enum USAStateName
{
    AL,
    AK,
    AZ,
    AR,
    CA,
    CO,
    CT,
    DE,
    FL,
    GA,
    HI,
    ID,
    IL,
    IN,
    IA,
    KS,
    KY,
    LA,
    ME,
    MD,
    MA,
    MI,
    MN,
    MS,
    MO,
    MT,
    NE,
    NV,
    NH,
    NJ,
    NM,
    NY,
    NC,
    ND,
    OH,
    OK,
    OR,
    PA,
    RI,
    SC,
    SD,
    TN,
    TX,
    UT,
    VT,
    VA,
    WA,
    WV,
    WI,
    WY
}

public class State {

    public USAStateName Name { get; private set; }
    public List<Tweet> Tweets { get; private set; } = new();

    public double Weight {
        get {
            double totalWeight = 0;
            foreach (var tweet in Tweets)
                totalWeight += tweet.Weight;
            return totalWeight;
        }
    }

    public State() {}

    public State(USAStateName name, List<Tweet> tweets) {
        Name = name;
        Tweets = tweets;
    }

    public override string ToString() {
        var str = new StringBuilder();
        str.AppendLine(Name.ToString());
        foreach (var tweet in Tweets)
            str.AppendLine(tweet.ToString());
        return str.ToString();
    }
}
