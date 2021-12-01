using System.ComponentModel.DataAnnotations;
using EngUoM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace EngUoM.Controllers {
 

        [Route("[controller]")]
        [ApiController]
        public class MeasureApiController : ControllerBase {
            private APIModel _apiModel;
            private ConvertUOM _convertUom;
            private readonly ILogger<MeasureApiController> _logger;

            public MeasureApiController(ILogger<MeasureApiController> logger) {
                _logger = logger;
                _apiModel = new APIModel();
                _convertUom =
                    new ConvertUOM();
            }

            [HttpGet("List Of Dimensions")]
            public ActionResult listAllDimensionClasses() => Ok(_apiModel.ListAllDimension());

            [HttpGet("List Of Quantities")]
            public ActionResult quantity() => Ok(_apiModel.ListAllQuantity());

            [HttpPost("UnitOfMeasureGivenDimensionClass")]
            [Produces("application/json")]
            [DataType("json")]
            public ActionResult GetUomForDimensionClass(PostDClass data) =>
                Ok(_apiModel.UOM_GClass(data.dimensionClassName));

            // Given a quantity class : and then return all the unit measurment for this particular quantity
            [HttpPost("UnitOfMeasureGivenQuantityClass")]
            [Produces("application/json")]
            [DataType("json")]
            public ActionResult UnitOfMeasuremetnQuantityClass(PostQClass data) =>
                Ok(_apiModel.UnitOfMeasure_GivenQuantityType(data.quantityClassName));

            [HttpPost("UOMConversion")]
            [Produces("application/json")]
            [DataType("json")]
            public ActionResult conversion(PostData data) =>
                Ok(_convertUom.Conversion(data.input_value, data.FromUom, data.ToUom));
        }
    }
