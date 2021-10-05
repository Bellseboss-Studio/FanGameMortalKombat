using System.Collections.Generic;
using MenuUI.SystemOfExtras;

public interface ICatalog
{
    List<IExtra> GetListOfExtras { get; }
    void AddExtra(Extra extra);
    void SaveData();
}