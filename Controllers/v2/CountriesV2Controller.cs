using HotelListing.Data;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers.v2;

[ApiVersion("2.0", Deprecated = true)]
[Route("api/{v:apiversion}/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    // don't do this normally; don't let controller go straight to database
    private DatabaseContext _databaseContext;

    public CountriesController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountries()
    {
        return Ok(_databaseContext.Countries);
    }
}
