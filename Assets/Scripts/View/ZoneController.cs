using System;
using ServiceLocatorPath;
using UnityEngine;
using View.Installers;
using View.Zone;

namespace View
{
    public class ZoneController : MonoBehaviour
    {
        [SerializeField] private string nameOfZone;
        [SerializeField] private AreaZoneController yellowZone, greenZone;
        public string NameZone => nameOfZone;

        private void Start()
        {
            ServiceLocator.Instance.GetService<IGodObserver>().Observe(nameOfZone, Zones.GREEN, greenZone);
            ServiceLocator.Instance.GetService<IGodObserver>().Observe(nameOfZone, Zones.YELLOW, yellowZone);
        }
    }
}