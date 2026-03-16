using SkiaSharp;

public class CountryDrawer {

    private string DataPath = String.Empty;

    private StateDrawer stateDrawer;

    public CountryDrawer(string dataPath) {
        DataPath = dataPath;

        stateDrawer = new StateDrawer(Path.Combine(dataPath, "states.json"));
    }

    public void DrawCountry(Country country) {
        var colorPicker = new ColorPicker(country);

        var width = 2000;
        var height = 1200;

        var bitmap = new SKBitmap(width, height);
        var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        foreach (var kvp in country.States) {
            stateDrawer.DrawState(
                colorPicker.GetColor(kvp.Value),
                kvp.Key,
                canvas,
                width,
                height
            );
        }

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        File.WriteAllBytes(Path.Combine(DataPath, "map.png"), data.ToArray());
    }
}
