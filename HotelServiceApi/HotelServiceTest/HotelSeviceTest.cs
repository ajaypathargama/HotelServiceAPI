using HotelServiceApi.Controllers;
using HotelServiceApi.Interface;
using HotelServiceApi.Model;
using HotelServiceApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HotelServiceTest
{
    public class HotestServiceTest
    {      
        private IHotelFacilityService hotelFacilityService;
        private ServiceCollection services;

        [SetUp]
        public void Setup()
        {
            services = new ServiceCollection();
            services.AddTransient<IHotelFacilityService, HotelFacilityService>();

            var serviceProvider = services.BuildServiceProvider();
            hotelFacilityService = serviceProvider.GetService<IHotelFacilityService>();           
        }

        [Test]
        public void TestGetHotelResult()
        {
          
            int expectedCount = 26;
            List<HotelRate> rates =hotelFacilityService.GetRateListByHotelIdAndArrivalDate(7294, "2016-03-15T00:00:00.000+01:00", @"F:\2021\HotelService\HotelServiceApi\HotelServiceApi\uploads\");
            int actualRecordCount = rates.Count;

            Assert.That(actualRecordCount, Is.EqualTo(expectedCount));            
        }

        //Negative test case
        [Test]
        public void TestGetHotelResultForInvalidPath()
        {

            Assert.Throws<FileNotFoundException>(() => hotelFacilityService.GetRateListByHotelIdAndArrivalDate(7294, "2016-03-15T00:00:00.000+01:00", @"F:\2021\HotelService\HotelServiceApi\HotelServiceApi\upload22\"));     
        }

        [Test]
        public async Task TestFileSave()
        {
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "this is Fake File to test";
            var fileName = "test.json";
            string filepath = @"F:\2021\HotelService\HotelServiceApi\HotelServiceApi\uploads\";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            
            var file = fileMock.Object;
            bool isSuccessful = await hotelFacilityService.SaveUploadedFile(file, filepath);

            Assert.IsTrue(isSuccessful);
           
        }


    }
}