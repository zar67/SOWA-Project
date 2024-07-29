using Mapbox.Unity.Location;

public class BinRecordingManager : SingletonMonoBehaviour<BinRecordingManager>
{
    public const string BINS_KEY = "MISSING_BINS_DATA";

    private AbstractLocationProvider m_locationProvider = null;

    public void RequestNewBinLocation()
    {
        if (m_locationProvider == null)
        {
            m_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
        }

        Location currentLocation = m_locationProvider.CurrentLocation;
        FirebaseDatabaseManager.Instance.AppendData(BINS_KEY, $"{currentLocation.LatitudeLongitude.x},{currentLocation.LatitudeLongitude.y}");
    }
}
