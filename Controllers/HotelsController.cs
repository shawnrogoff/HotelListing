﻿using AutoMapper;
using HotelListing.Core.Dto;
using HotelListing.Core.IRepository;
using HotelListing.Core.Models;
using HotelListing.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HotelsController> _logger;
    private readonly IMapper _mapper;

    public HotelsController(IUnitOfWork unitOfWork, ILogger<HotelsController> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetHotels([FromQuery] RequestParams requestParams)
    {
        var hotels = await _unitOfWork.Hotels.GetPagedList(requestParams);
        var results = _mapper.Map<IList<HotelDTO>>(hotels);
        return Ok(results);
    }


    [HttpGet("{id:int}", Name = "GetHotel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetHotel(int id)
    {
        var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id, new List<string> { "Country" });
        var result = _mapper.Map<HotelDTO>(hotel);
        return Ok(result);
    }


    [Authorize(Roles = "Administrator")] // Only admin can create a hotel
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
            return BadRequest(ModelState);
        }

        var hotel = _mapper.Map<Hotel>(hotelDTO);
        await _unitOfWork.Hotels.Insert(hotel);
        await _unitOfWork.Save();

        return CreatedAtRoute("GetHotel", new { id = hotel.Id });
    }


    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO updateHotelDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
            return BadRequest(ModelState);
        }

        var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
        if (hotel == null)
        {
            _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
            return BadRequest("Submitted data is invalid");
        }

        _mapper.Map(updateHotelDTO, hotel); //(<source>, <destination>)
        _unitOfWork.Hotels.Update(hotel);
        await _unitOfWork.Save();

        return NoContent();
    }


    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        if (id < 1)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
            return BadRequest();
        }

        var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
        if (hotel == null)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
            return BadRequest("Submitted data is invalid");
        }

        await _unitOfWork.Hotels.Delete(hotel.Id);
        await _unitOfWork.Save();

        return NoContent();
    }
}
