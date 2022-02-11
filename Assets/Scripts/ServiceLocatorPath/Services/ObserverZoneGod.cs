using System;
using System.Collections.Generic;
using View.Zone;

namespace ServiceLocatorPath
{
    public class ObserverZoneGod : IGodObserver
    {
        private Dictionary<string, Dictionary<Zones, AreaZoneController>> allZones;

        public ObserverZoneGod()
        {
            allZones = new Dictionary<string, Dictionary<Zones, AreaZoneController>>();
        }

        public void Observe(string nameOfZone, Zones zone, AreaZoneController area)
        {
            if (allZones.ContainsKey(nameOfZone))
            {
                var dic = allZones[nameOfZone];
                dic.Add(zone,area);
            }
            else
            {
                var newZoneWithArea = new Dictionary<Zones, AreaZoneController> { { zone, area } };
                allZones.Add(nameOfZone, newZoneWithArea);
            }
        }

        public AreaZoneController GetZone(string nameOfZone, Zones zone)
        {
            if (!allZones.ContainsKey(nameOfZone))
            {
                throw new Exception("The nameOfZone dont exist");
            }
            var areaZoneControllers = allZones[nameOfZone];
            if (!areaZoneControllers.ContainsKey(zone))
            {
                throw new Exception("The zone dont exist");
            }
            return areaZoneControllers[zone];
        }

        public void UnObserve()
        {
            allZones = new Dictionary<string, Dictionary<Zones, AreaZoneController>>();           
        }
    }
}