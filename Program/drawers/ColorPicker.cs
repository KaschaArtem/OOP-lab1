using SkiaSharp;

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

    public SKColor GetColor(State state) {
        if (state.IsEmpty())
            return new SKColor(191, 191, 191);

        var weight = state.Weight;
        double n = (weight - MinWeight) / (MaxWeight - MinWeight);
        if (n < 0.5)
            return new SKColor(
                255,
                (byte)(255 * (1 - n)),
                (byte)(255 * (1 - n))
            );
        return new SKColor(
            (byte)(255 * (1 - n)),
            255,
            (byte)(255 * (1 - n))
        );
    }
}
