using System.Collections.Generic;


namespace HotelServiceApi.Model
{
    public class HotelInformation
    {
        public Hotel hotel { get; set; }
        public List<HotelRate> hotelRates { get; set; }
    }
}
