using HotelServiceApi.Common;
using HotelServiceApi.Interface;
using HotelServiceApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HotelServiceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelServiceController : ControllerBase
    {     

        private IHostEnvironment _hostingEnvironment;        
        private IHotelFacilityService _hotelFacilityService;

        public HotelServiceController(IHostEnvironment environment, IHotelFacilityService hotelFacilityService)
        {           
            _hostingEnvironment = environment;
            _hotelFacilityService = hotelFacilityService;
        }


        /// <summary>
        /// Upload the selected file on the server
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);
            try
            {
                bool isSaved = await _hotelFacilityService.SaveUploadedFile(file, uploads);
                if (isSaved)
                    return Ok(Constants.SUCCESSFUL_UPLOAD);

                return BadRequest(Constants.FAILED_TO_UPLOAD);
            }
            catch (Exception ex)
            {
                //log exception here
                return BadRequest(Constants.FAILED_TO_UPLOAD);
            }
        }
            /// <summary>
            /// Get the rate deatils for the given hotel id and arrival date
            /// </summary>
            /// <param name="hotelId"></param>
            /// <param name="arrivalDate"></param>
            /// <returns>It returns the list of all rate details which matches for given hotel id and arrival date</returns>
            [HttpGet("GetRate")]
            public IActionResult GetRate(int hotelId, string arrivalDate)
            {
                    try
                    {
                        string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");
                        List<HotelRate> rates = _hotelFacilityService.GetRateListByHotelIdAndArrivalDate(hotelId, arrivalDate, uploads);

                        return Ok(rates);
                    }
                    catch(Exception ex)
                    {
                        //log exception here
                        return BadRequest(ex.Message);
                    }
            }       
            
        }
    }  
   



