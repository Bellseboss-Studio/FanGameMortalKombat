using View.Zone;

namespace ServiceLocatorPath
{
    public interface IGodObserver
    {
        void Observe(string nameOfZone, Zones zone, AreaZoneController area);
        AreaZoneController GetZone(string nameOfZone, Zones zone);
        void UnObserve();
    }
}