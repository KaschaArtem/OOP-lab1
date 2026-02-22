using System.Text;

public class Country {

    public List<State> States { get; private set; } = new();

    public Country() {}

    public Country(List<State> states) {
        States = states;
    }

    public override string ToString() {
        var str = new StringBuilder();
        foreach (var state in States)
            str.AppendLine(state.ToString());
        return str.ToString();
    }
}
