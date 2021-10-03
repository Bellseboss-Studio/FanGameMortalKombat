using System.Collections;
using System.Collections.Generic;

namespace MenuUI.SystemOfExtras
{
    public class Catalog
    {
        private readonly ISaveData _saveData;
        private List<IExtra> listOfExtras;

        public Catalog(ISaveData saveData)
        {
            _saveData = saveData;
            listOfExtras = !saveData.HasData() ? saveData.CreateData() : saveData.LoadData();
        }

        public List<IExtra> GetListOfExtras => listOfExtras;

        public void SaveData()
        {
            _saveData.SaveData(listOfExtras);
        }

        public void AddExtra(Extra extra)
        {
            var extraCompont = new ImageComponentExtra(extra);
            listOfExtras.Add(extraCompont);
        }
    }
}