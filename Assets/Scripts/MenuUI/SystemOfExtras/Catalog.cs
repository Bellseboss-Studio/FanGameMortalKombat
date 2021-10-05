using System.Collections;
using System.Collections.Generic;
using ServiceLocatorPath;

namespace MenuUI.SystemOfExtras
{
    public class Catalog : ICatalog
    {
        private readonly ISaveData _saveData;
        private List<IExtra> listOfExtras;
        private string catalog = "Extras";
        private PlayFabLogin login;

        public Catalog(ISaveData saveData)
        {
            _saveData = saveData;
            FunctionVoid();
        }

        private async void FunctionVoid()
        {
            var respuesta = await _saveData.HasData();
            listOfExtras = !respuesta ? _saveData.CreateData() : _saveData.LoadData();
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