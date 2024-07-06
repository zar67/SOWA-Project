using System;
using System.Collections.Generic;

public class UserStatistics
{
    [NonSerialized] public long TotalRecordedLitter = 0;
    public Dictionary<string, long> RecordedLitterByTag = new Dictionary<string, long>();
}
