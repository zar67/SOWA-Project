using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections.Generic;

public class LitterData
{
    public string Timestamp;
    public string Location;
    public string[] Tags;
    [NonSerialized] public int MergedAmount = 1;

    public LitterData Merge(LitterData other)
    {
        var newData = new LitterData()
        {
            Timestamp = other.Timestamp
        };

        newData.MergedAmount = MergedAmount + other.MergedAmount;

        var tags = new List<string>(Tags);
        tags.AddRange(other.Tags);
        newData.Tags = tags.ToArray();

        Vector2d thisLocation = Conversions.StringToLatLon(Location);
        Vector2d otherLocation = Conversions.StringToLatLon(other.Location);

        Vector2d mergedLocation = (thisLocation + otherLocation) / 2;

        newData.Location = $"{mergedLocation.x},{mergedLocation.y}";
        return newData;
    }
}
