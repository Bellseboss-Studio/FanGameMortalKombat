using System.Collections.Generic;

namespace MenuUI.SystemOfExtras
{
    public interface ISaveData
    {
        bool HasData();
        List<IExtra> CreateData();
        List<IExtra> LoadData();
        void SaveData(List<IExtra> listOfExtras);
    }
}