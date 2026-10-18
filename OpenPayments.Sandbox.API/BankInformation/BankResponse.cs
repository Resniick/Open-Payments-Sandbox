using System.Text.Json.Serialization;
using OpenPayments.Sandbox.Core;

namespace OpenPayments.Sandbox.API.BankInformation;

public record BankResponse
{
	[JsonPropertyName("name")]
	public required string Name { get; init; }
	
	[JsonPropertyName("bicFi")]
	public required string SwiftCode { get; init; }
	
	[JsonPropertyName("logoUrl")]
	public string? LogoUrl { get; init; }

	public static BankResponse FromBank(Bank bank)
	{
		return new BankResponse
		{
			Name = bank.Name,
			SwiftCode = bank.SwiftCode,
			LogoUrl = null,
		};
	}
}
