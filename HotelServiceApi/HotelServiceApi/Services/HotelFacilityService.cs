using HotelServiceApi.Common;
using HotelServiceApi.Interface;
using HotelServiceApi.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HotelServiceApi.Services
{
    public class HotelFacilityService : IHotelFacilityService
    {
        public List<HotelRate> GetRateListByHotelIdAndArrivalDate(int hotelId, string arrivalDate, string filePath)
        {
            List<HotelRate> rates = null;
            string fullPath = null;
            if(Directory.Exists(filePath))
            {
                fullPath = getLatestFileNamefromDirectory(filePath);
                fullPath =   filePath + @"\" + fullPath;
            }
            
            if (File.Exists(fullPath))
            {                

                List<HotelInformation> result = JsonSerializer.Deserialize<List<HotelInformation>>(File.ReadAllText(fullPath));              
                var item = result.FirstOrDefault(x => x.hotel.hotelID == hotelId);
                if (item != null)
                {                    
                    DateTime arrivalDateTime;
                    if (DateTime.TryParse(arrivalDate, out arrivalDateTime))
                    {
                        rates = new();                     
                        foreach (var hotelRate in item.hotelRates)
                        {
                            DateTime dateTime;
                            if (DateTime.TryParse(Convert.ToString(hotelRate.targetDay), out dateTime) && dateTime.Date.Equals(arrivalDateTime.Date))
                            {
                                rates.Add(hotelRate);
                            }
                        }                        
                    }
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
            return rates;
        }

        /// <summary>
        /// Saves file in the target file path
        /// </summary>
        /// <param name="file"></param>
        /// <param name="target_filePath"></param>
        /// <returns>returns true if file successfully saved</returns>
        public async Task<bool> SaveUploadedFile(IFormFile file, string target_filePath)
        {
            try
            {
                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        string filePath = Path.Combine(target_filePath, file.FileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        return true;
                    }
                }
                return false;
            }
            catch(Exception ex)
            {
                throw;
            }            
        }

        /// <summary>
        /// gets the latest file name from the directory
        /// </summary>
        /// <returns>latest supported file name</returns>
        private string getLatestFileNamefromDirectory(string filePath)
        {
            try
            {
                string pattern = "*" + Constants.SUPPORTED_FILE_TYPE;
                var dirInfo = new DirectoryInfo(filePath);
                var file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).FirstOrDefault();
                return file.Name;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }    
}
