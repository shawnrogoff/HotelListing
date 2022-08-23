﻿using System.ComponentModel.DataAnnotations;

namespace HotelListing.Dto;

public class CountryDTO : CreateCountryDTO
{
    public int Id { get; set; }
    public virtual IList<HotelDTO> Hotels { get; set; }
}

public class CreateCountryDTO
{
    [Required]
    [StringLength(maximumLength: 50, ErrorMessage = "Country Name Is Too Long")]
    public string Name { get; set; }
    [Required]
    [StringLength(maximumLength: 2, ErrorMessage = "Short Country Name Is Too Long")]
    public string ShortName { get; set; }
}
