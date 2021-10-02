using System;
using MenuUI.SystemOfExtras;

[Serializable]
public class Extra
{
  public string name;
  public string source;
  public string type;
  public string icon;

  public Extra()
  {
  }

  public Extra(IExtra extra)
  {
    name = extra.GetName();
    source = extra.GetSource();
    type = extra.GetTypeExtra();
    icon = extra.GetIcon();
  }
}
/*
{
  "name":"texto",
  "source": "URL",
  "type": "texto",
  "icon":"URL"
}
*/