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

    public Dictionary<USAStateName, State> States { get; private set; } = new();

    public Country() {}

    public Country(Dictionary<USAStateName, State> states) {
        States = states;
    }

    public double GetStateValue(USAStateName state) {
        return States[state].Weight;
    }

    public override string ToString() {
        var str = new StringBuilder();
        foreach (var kvp in States) {
            str.AppendLine(kvp.Key.ToString());
            str.AppendLine(kvp.Value.ToString());
        }
        return str.ToString();
    }
}
