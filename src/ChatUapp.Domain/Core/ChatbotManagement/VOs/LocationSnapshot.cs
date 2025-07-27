using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace ChatUapp.Core.ChatbotManagement.VOs;

public class LocationSnapshot : ValueObject
{
    public string CountryName { get; } = string.Empty;
    public double Longitude { get; } 
    public double Latitude { get; }
    public string Flag { get; } = string.Empty;
    public string Ip { get; } = string.Empty;

    private LocationSnapshot() { } // For EF Core

    public LocationSnapshot(string name, double longitude, double latitude, int users, string flag, string ip)
    {
        CountryName = name;
        Longitude = longitude;
        Latitude = latitude;
        Flag = flag;
        Ip = ip;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return CountryName;
        yield return Longitude;
        yield return Latitude;
        yield return Flag;
        yield return Ip;
    }
}
