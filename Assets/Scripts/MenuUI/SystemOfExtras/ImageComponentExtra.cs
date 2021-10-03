using MenuUI.SystemOfExtras;

public class ImageComponentExtra : IExtra
{
    private string name;
    private string source;
    private string type;
    private string icon;
    public ImageComponentExtra(Extra extra)
    {
        name = extra.name;
        source = extra.source;
        type = extra.type;
        icon = extra.icon;
    }

    public void ShowContent()
    {
        throw new System.NotImplementedException();
    }

    public string GetName()
    {
        return name;
    }

    public string GetSource()
    {
        return source;
    }

    public string GetTypeExtra()
    {
        return type;
    }

    public string GetIcon()
    {
        return icon;
    }
}