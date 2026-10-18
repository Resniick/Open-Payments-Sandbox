using System.Text.Json.Serialization;

namespace OpenPayments.Sandbox.API.BankInformation;

public record BankListResponse
{
	[JsonPropertyName("aspsps")]
	public required IReadOnlyList<BankResponse> Banks { get; init; }
}
