using Mapbox.Unity.Location;
using Mapbox.Utils;
using TMPro;
using UnityEngine;

public class CoordinatesDisplay : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI m_statusText;

	private AbstractLocationProvider m_locationProvider = null;

    private void Update()
	{
		if (m_locationProvider == null)
		{
			m_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
		}

		Location currentLocation = m_locationProvider.CurrentLocation;

		if (currentLocation.IsLocationServiceInitializing)
		{
			m_statusText.text = "Initializing Location Services...";
		}
		else
		{
			if (!currentLocation.IsLocationServiceEnabled)
			{
				m_statusText.text = "Location Services not enabled";
			}
			else
			{
				if (currentLocation.LatitudeLongitude.Equals(Vector2d.zero))
				{
					m_statusText.text = "Waiting for location ....";
				}
				else
				{
					m_statusText.text = currentLocation.LatitudeLongitude.ToString();
				}
			}
		}

	}
}
