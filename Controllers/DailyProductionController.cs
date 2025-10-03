using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DailyProduction.Models;

namespace IbasAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DailyProductionController : ControllerBase
    {

        private List<DailyProductionDTO> _productionRepo;
        private readonly ILogger<DailyProductionController> _logger;

        public DailyProductionController(ILogger<DailyProductionController> logger)
        {
            _logger = logger;
            /*
            _productionRepo = new List<DailyProductionDTO>
            {
                new DailyProductionDTO {Date = new DateTime(2020, 1, 31), Model = BikeModel.IBv1, ItemsProduced = 12},
                new DailyProductionDTO {Date = new DateTime(2020, 2, 28), Model = BikeModel.IBv1, ItemsProduced = 32},
                new DailyProductionDTO {Date = new DateTime(2020, 3, 31), Model = BikeModel.IBv1, ItemsProduced = 32},
                new DailyProductionDTO {Date = new DateTime(2020, 4, 30), Model = BikeModel.IBv1, ItemsProduced = 141},
                new DailyProductionDTO {Date = new DateTime(2020, 5, 31), Model = BikeModel.IBv1, ItemsProduced = 146},
                new DailyProductionDTO {Date = new DateTime(2020, 6, 30), Model = BikeModel.IBv1, ItemsProduced = 162},
                new DailyProductionDTO {Date = new DateTime(2020, 7, 31), Model = BikeModel.IBv1, ItemsProduced = 102},
                new DailyProductionDTO {Date = new DateTime(2020, 8, 31), Model = BikeModel.IBv1, ItemsProduced = 210},
                new DailyProductionDTO {Date = new DateTime(2020, 9, 30), Model = BikeModel.IBv1, ItemsProduced = 144},
                new DailyProductionDTO {Date = new DateTime(2020, 10, 31), Model = BikeModel.IBv1, ItemsProduced = 151},
                new DailyProductionDTO {Date = new DateTime(2020, 11, 30), Model = BikeModel.IBv1, ItemsProduced = 61},
                new DailyProductionDTO {Date = new DateTime(2020, 12, 31), Model = BikeModel.IBv1, ItemsProduced = 86},

                new DailyProductionDTO {Date = new DateTime(2020, 1, 31), Model = BikeModel.evIB100, ItemsProduced = 1},
                new DailyProductionDTO {Date = new DateTime(2020, 2, 28), Model = BikeModel.evIB100, ItemsProduced = 2},
                new DailyProductionDTO {Date = new DateTime(2020, 3, 31), Model = BikeModel.evIB100, ItemsProduced = 3},
                new DailyProductionDTO {Date = new DateTime(2020, 4, 30), Model = BikeModel.evIB100, ItemsProduced = 4},
                new DailyProductionDTO {Date = new DateTime(2020, 5, 31), Model = BikeModel.evIB100, ItemsProduced = 4},
                new DailyProductionDTO {Date = new DateTime(2020, 6, 30), Model = BikeModel.evIB100, ItemsProduced = 6},
                new DailyProductionDTO {Date = new DateTime(2020, 7, 31), Model = BikeModel.evIB100, ItemsProduced = 10},
                new DailyProductionDTO {Date = new DateTime(2020, 8, 31), Model = BikeModel.evIB100, ItemsProduced = 21},
                new DailyProductionDTO {Date = new DateTime(2020, 9, 30), Model = BikeModel.evIB100, ItemsProduced = 17},
                new DailyProductionDTO {Date = new DateTime(2020, 10, 31), Model = BikeModel.evIB100, ItemsProduced = 15},
                new DailyProductionDTO {Date = new DateTime(2020, 11, 30), Model = BikeModel.evIB100, ItemsProduced = 25},
                new DailyProductionDTO {Date = new DateTime(2020, 12, 31), Model = BikeModel.evIB100, ItemsProduced = 30},

                new DailyProductionDTO {Date = new DateTime(2020, 1, 31), Model = BikeModel.evIB200, ItemsProduced = 10},
                new DailyProductionDTO {Date = new DateTime(2020, 2, 28), Model = BikeModel.evIB200, ItemsProduced = 2},
                new DailyProductionDTO {Date = new DateTime(2020, 3, 31), Model = BikeModel.evIB200, ItemsProduced = 32},
                new DailyProductionDTO {Date = new DateTime(2020, 4, 30), Model = BikeModel.evIB200, ItemsProduced = 41},
                new DailyProductionDTO {Date = new DateTime(2020, 5, 31), Model = BikeModel.evIB200, ItemsProduced = 46},
                new DailyProductionDTO {Date = new DateTime(2020, 6, 30), Model = BikeModel.evIB200, ItemsProduced = 62},
                new DailyProductionDTO {Date = new DateTime(2020, 7, 31), Model = BikeModel.evIB200, ItemsProduced = 102},
                new DailyProductionDTO {Date = new DateTime(2020, 8, 31), Model = BikeModel.evIB200, ItemsProduced = 21},
                new DailyProductionDTO {Date = new DateTime(2020, 9, 30), Model = BikeModel.evIB200, ItemsProduced = 44},
                new DailyProductionDTO {Date = new DateTime(2020, 10, 31), Model = BikeModel.evIB200, ItemsProduced = 51},
                new DailyProductionDTO {Date = new DateTime(2020, 11, 30), Model = BikeModel.evIB200, ItemsProduced = 61},
                new DailyProductionDTO {Date = new DateTime(2020, 12, 31), Model = BikeModel.evIB200, ItemsProduced = 88}
            }; */
        }
        
        /*
        [HttpGet]
        public IEnumerable<DailyProductionDTO> Get()
        {
            return _productionRepo;
        }
        */
        
        [HttpGet]
        public IEnumerable<DailyProductionDTO> Get()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "IBASProduction2022.csv");

            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogError("CSV file not found at " + filePath);
                return new List<DailyProductionDTO>();
            }

            var lines = System.IO.File.ReadAllLines(filePath).Skip(1); // spring header over
            var list = new List<DailyProductionDTO>();

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                // parts[0] = PartitionKey
                // parts[1] = RowKey (dato)
                // parts[2] = ProductionTime (ignoreres)
                // parts[3] = itemsProduced
                // parts[4] = itemsProduced@type (ignoreres)

                int partitionKey = int.Parse(parts[0]);
                DateTime date = DateTime.Parse(parts[1]);
                int produced = int.Parse(parts[3]);

                BikeModel model = BikeModel.undefined;
                if (partitionKey == 1) model = BikeModel.IBv1;
                else if (partitionKey == 2) model = BikeModel.evIB100;
                else if (partitionKey == 3) model = BikeModel.evIB200;

                var dto = new DailyProductionDTO
                {
                    Date = date,
                    Model = model,
                    ItemsProduced = produced
                };

                list.Add(dto);
            }

            return list;
        }
    }
}