using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<StopController> _logger;
        private GeoLocService _geoLocService;

        public StopController(IWorldRepository repository, ILogger<StopController> logger, GeoLocService geoLocService)
        {
            _repository = repository;
            _logger = logger;
            _geoLocService = geoLocService;
        }

        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try
            {
                var result = _repository.GetTripByName(tripName, User.Identity.Name);

                if (result == null)
                {
                    return Json(null);
                }

                return Json(Mapper.Map<IEnumerable<StopViewModel>>(result.Stops.OrderBy(s => s.Order)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't get trip {tripName}", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
           
        }

        [HttpPost("")]
        public async Task<JsonResult> Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(vm);

                    var coordResult = await _geoLocService.Lookup(newStop.Name);

                    if (!coordResult.Success)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(coordResult.Message);
                    }

                    // Set coordinates
                    newStop.Latitude = coordResult.Latitude;
                    newStop.Longitude = coordResult.Longitude;

                    // Saveing a stop
                    _repository.AddStop(tripName, User.Identity.Name, newStop);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<StopViewModel>(newStop));
                    }
                }
              
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Validation was failed");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't save new stop", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }
    }
}
