using HotelServiceApi.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelServiceApi.Interface
{
    public interface IHotelFacilityService
    {
        Task<bool> SaveUploadedFile(IFormFile file,string target_filePath);

        List<HotelRate> GetRateListByHotelIdAndArrivalDate(int hotelId, string arrivalDate, string filePath);
    }
}
