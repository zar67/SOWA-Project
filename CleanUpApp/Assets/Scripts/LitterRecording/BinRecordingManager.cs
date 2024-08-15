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

        PopupManager.Instance.OpenPopup(new GenericInfoPopupData()
        {
            BodyText = "Thank you for recording a new bin location! It may take some time for your bin to appear on the map as adding a new bin is not an automatic process. Please be patient with us and your new bin will show soon!",
            ShowCloseButton = true,
            Type = PopupType.GENERIC_INFO
        });
    }
}
