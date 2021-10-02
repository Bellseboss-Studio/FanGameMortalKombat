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
    name = "texto para el ejemplo";
    source = "Aquellos que no lidian con si mismos, serán usados por otros a su propio favor... y estos últimos lograrán la iluminación a costa de los primeros desafortunados";
    type = "text";
    icon = "Text";
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