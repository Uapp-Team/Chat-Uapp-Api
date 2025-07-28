using System;
using System.ComponentModel.DataAnnotations;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Session;

public class CreateSessionInputDto 
{
    public Guid chatbotId { get;  set; }
    public string sessionTitle { get; set; } = default!;
    public LocationSnapshotDto LocationSnapshot { get; set; } = new LocationSnapshotDto();
    public string? BrowserSessionKey { get; set; }
}

public class LocationSnapshotDto
{
    [Required]
    public string CountryName { get; set; } = string.Empty;
    [Required]
    public double Longitude { get; set; }
    [Required]
    public double Latitude { get; set; }
    [Required]
    public string Flag { get; set; } = string.Empty;
    [Required]
    public string Ip { get; set; } = string.Empty;
}