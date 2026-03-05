using System.Drawing;

public class ColorPicker {

    public double MinWeight { get; private set; } = double.MaxValue;
    public double MaxWeight { get; private set; } = double.MinValue;

    public ColorPicker(Country country) {
        CountMinMaxWeight(country);
    }

    private void CountMinMaxWeight(Country country) {
        foreach (var state in country.States.Values) {
            double weight = state.Weight;
            if (weight < MinWeight)
                MinWeight = weight;
            if (weight > MaxWeight)
                MaxWeight = weight;
        }
    }

    public Color GetColor(double weight) {
        return Color.White;
    }
}
