using FgssrApi.Dtos;
using FgssrApi.UnitOFWork;
using FgssrApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace FgssrApi.Controllers
{
    //[Authorize(Roles = "Admin")]

    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public EventsController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }



        [HttpGet]
        [Route("GetAllEvents")]
        public async Task<IActionResult> GetAllEvents()
        {

            var events = await _unitofwork.Events.GetAllAsync();
            var evm = events.Select(eve => new EventsDto
            {
                EveId = eve.EventId,
                EventTitle = eve.EventTitle,
                EventDescription = eve.EventDescription,
                DateDay = eve.DateDay,
                EventLocation = eve.EventLocation,
                DateMonth = eve.DateMonth,
                EventImageName=eve.EventImage,
                Time=eve.Time,

            }).ToList();

            return Ok(evm);
        }

        [HttpGet]
        [Route("GetEventsById/{id}")]
        public async Task<IActionResult> GetEventsById(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var events = await _unitofwork.Events.GetByIdAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            var evm = new EventsDto()
            {
                EveId = events.EventId,
                EventTitle = events.EventTitle,
                EventDescription = events.EventDescription,
                EventLocation = events.EventLocation,
                DateDay = events.DateDay,
                DateMonth = events.DateMonth,
                EventImageName = events.EventImage,
                Time = events.Time,
            };
            return Ok(evm);
        }

        [HttpDelete]
        [Route("DeleteEventById/{id}")]
        public async Task<IActionResult> DeleteEvent(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var events = await _unitofwork.Events.GetByIdAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            _unitofwork.Events.Delete(events);
            var imageDelete = Path.Combine(_unitofwork.Events.uploadFolderPublic, events.EventImage);
            System.IO.File.Delete(imageDelete);
            _unitofwork.SaveChanges();

            return Ok("Delete is successfull");
        }


      

        [HttpPost]
        [Route("CreateNewEvent")]
        public async Task<IActionResult> CreateEvent([FromForm] EventsDtoCU EVM)
        {


           var eve = await _unitofwork.Events.CreateAsync(EVM);
            _unitofwork.SaveChanges();

            return CreatedAtAction(nameof(GetEventsById), new { id = eve.EventId }, EVM);
        }




        [HttpPut]
        [Route("UpdateEventById/{id}")]
        public async Task<IActionResult> UpdateEvent(int? id, [FromForm] EventsDtoCU EVM)
        {

            if (id == null)
            {
                return BadRequest();
            }


            var events = await _unitofwork.Events.GetByIdAsync(id);

            if (events == null)
            {
                return NotFound();
            }

            var oldImage = events.EventImage;

            events.EventTitle = EVM.EventTitle;
            events.EventDescription = EVM.EventDescription;
            events.DateDay = EVM.DateDay;
            events.EventLocation = EVM.EventLocation;
            events.DateMonth = EVM.DateMonth;
            events.Time = EVM.Time;
            if(EVM.UploadImage != null)
            {
                events.EventImage = await _unitofwork.Events.SaveNewImage(EVM.UploadImage);
                var oldImageDelete = Path.Combine(_unitofwork.Events.uploadFolderPublic, oldImage);
                System.IO.File.Delete(oldImageDelete);
            }
                       
            _unitofwork.SaveChanges();

            return Ok(events);
        }

    }
}
