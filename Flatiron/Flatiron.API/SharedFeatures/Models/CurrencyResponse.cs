using System.Text.Json.Serialization;

namespace Flatiron.API.SharedFeatures.Models;

public record CurrencyResponse([property: JsonPropertyName("date")] string? Date,
                               [property: JsonPropertyName("usd")] Dictionary<string?, decimal?> Usd);
