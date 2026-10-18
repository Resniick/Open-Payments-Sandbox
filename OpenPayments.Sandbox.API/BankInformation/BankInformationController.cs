using Microsoft.AspNetCore.Mvc;
using OpenPayments.Sandbox.Core;

namespace OpenPayments.Sandbox.API.BankInformation;

[ApiController]
[Route("psd2/aspspinformation/v1/aspsps")]
[Tags("Bank information")]
public class BankInformationController : ControllerBase
{
	private readonly IBankRepository _bankRepository;
	
	public BankInformationController(IBankRepository bankRepository)
	{
		this._bankRepository = bankRepository;
	}
	
	[HttpGet]
	[EndpointName("GetAspspList")]
	[EndpointSummary("Get ASPSP List")]
	[EndpointDescription("Gets all ASPSPs currently supported in the Open Banking Platform.")]
	[ProducesResponseType<BankListResponse>(StatusCodes.Status200OK)]
	public async Task<ActionResult<BankListResponse>> GetAll()
	{
		var banks = await this._bankRepository.GetAll();
		
		return this.Ok(new BankListResponse
		{
			Banks = [..banks.Select(BankResponse.FromBank)],
		});
	}
	
	[HttpGet("{bic}")]
	[EndpointName("GetAspspDetails")]
	[EndpointSummary("Get ASPSP Details")]
	[EndpointDescription("Gets details about an ASPSP by BIC.")]
	[ProducesResponseType<BankResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<BankResponse>> Get(string bic)
	{
		var bank = await this._bankRepository.Get(bic);

		return bank is null ? this.NotFound() : this.Ok(BankResponse.FromBank(bank.Value));
	}
}
