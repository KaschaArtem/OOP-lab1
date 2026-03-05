using System.Drawing;

public class CountryDrawer {

    private string DataPath = String.Empty;

    private StateDrawer stateDrawer;

    public CountryDrawer(string dataPath) {
        DataPath = dataPath;

        stateDrawer = new StateDrawer(Path.Combine(dataPath, "states.json"));
    }
}
