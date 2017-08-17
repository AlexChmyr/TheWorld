using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    public class TripController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<TripController> _logger;

        public TripController(IWorldRepository repository, ILogger<TripController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public JsonResult Get()
        {
            var trips = _repository.GetUserTripsWithStops(User.Identity.Name);
            var result = Mapper.Map<IEnumerable<TripViewModel>>(trips);
            return Json(result);
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]TripViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTrip = Mapper.Map<Trip>(vm);
                    newTrip.User = User.Identity.Name;

                    // Saveing
                    _logger.LogInformation("Saveing a new trip");
                    _repository.AddTrip(newTrip);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<TripViewModel>(newTrip));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't save trip", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message});
            }
            
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Request is failed");
        }
    }
}
