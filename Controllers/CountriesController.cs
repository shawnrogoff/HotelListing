using AutoMapper;
using HotelListing.Dto;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace HotelListing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CountriesController> _logger;
    private readonly IMapper _mapper;

    public CountriesController(IUnitOfWork unitOfWork, ILogger<CountriesController> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetCountries()
    {
        try
        {
            var countries = await _unitOfWork.Countries.GetAll();
            var results = _mapper.Map<IList<CountryDTO>>(countries);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something Went Wrong in the {nameof(GetCountries)}");
            return StatusCode(500, $"Internal Server Error. Please Try Again Later.");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCountry(int id)
    {
        try
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> {"Hotels"});
            var result = _mapper.Map<CountryDTO>(country);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something Went Wrong in the {nameof(GetCountry)}");
            return StatusCode(500, $"Internal Server Error. Please Try Again Later.");
        }
    }
}
