using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;

public class LitterData
{
    public string Timestamp;
    public string Location;

    public LitterData Merge(LitterData other)
    {
        var newData = new LitterData()
        {
            Timestamp = other.Timestamp
        };

        Vector2d thisLocation = Conversions.StringToLatLon(Location);
        Vector2d otherLocation = Conversions.StringToLatLon(other.Location);

        Vector2d mergedLocation = (thisLocation + otherLocation) / 2;

        newData.Location = $"{mergedLocation.x},{mergedLocation.y}";
        return newData;
    }
}
