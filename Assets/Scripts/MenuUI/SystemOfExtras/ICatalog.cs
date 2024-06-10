using System.Collections.Generic;
using System.Threading.Tasks;
using MenuUI.SystemOfExtras;

public interface ICatalog
{
    List<IExtra> GetListOfExtras { get; }
    void AddExtra(Extra extra);
    void SaveData();
    void SaveData(string itemId);
    void LoadCatalog();
    Task LoadDataCatalog();
}