using Microsoft.AspNetCore.Mvc;
using EmulatorsRequester.Data;
using EmulatorsRequester.Models;

namespace EmulatorsRequester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class sensorController : Controller
    {
        private readonly EmulatorSensorContext _context;

        public sensorController(EmulatorSensorContext context)
        {
            _context = context;
        }

        [HttpGet("{prefix}")]
        public async Task<ActionResult<Sensor>> GetSensor(string prefix)
        {
            var item = await _context.Sensors.FindAsync(prefix);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
    }
}
