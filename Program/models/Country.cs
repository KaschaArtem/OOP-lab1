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

public class Country {

    public Dictionary<USAStateName, List<Tweet>> States { get; private set; } = new();

    public Country() {}

    public Country(Dictionary<USAStateName, List<Tweet>> states) {
        States = states;
    }

    public override string ToString() {
        var str = new StringBuilder();
        foreach (var kvp in States) {
            str.AppendLine(kvp.Key.ToString());
            foreach (var tweet in kvp.Value)
                str.AppendLine(tweet.ToString());
        }
        return str.ToString();
    }
}
