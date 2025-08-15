using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisCacheExample.Cache;
using RedisCacheExample.Models;

namespace RedisCacheExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly RedisCacheDbContext _redisCacheDbContext;
        public EmployeeController(ICacheService cacheService, RedisCacheDbContext redisCacheDbContext)
        {
            _cacheService = cacheService;
            _redisCacheDbContext = redisCacheDbContext;
        }

        [HttpGet("Employees")]
        public IEnumerable<TblEmployee> Get() {
            var cacheddata = _cacheService.GetData<IEnumerable<TblEmployee>>("Employees");
            if (cacheddata != null) {
                return cacheddata;
            }
            var expirationTime = DateTime.Now.AddMinutes(5);
            cacheddata = _redisCacheDbContext.TblEmployees.ToList();
            _cacheService.SetData<IEnumerable<TblEmployee>>("Employees", cacheddata, expirationTime);
            return cacheddata;
        }
        [HttpGet("Employee/{id}")]
        public TblEmployee Get(int id) {
            var cacheddata = _cacheService.GetData<IEnumerable<TblEmployee>>("Employees");
            TblEmployee filtereddata;    
            if (cacheddata != null)
            {
                filtereddata = cacheddata.FirstOrDefault(x =>x.Id == id);
                return filtereddata;
            }
            filtereddata = _redisCacheDbContext.TblEmployees.FirstOrDefault(x => x.Id == id);
            return filtereddata;
        }

        [HttpPut("Update")]
        public void Update(TblEmployee employee)
        {
            _redisCacheDbContext.TblEmployees.Update(employee);
            _redisCacheDbContext.SaveChanges();
            _cacheService.RemoveData("Employees");
        }

        [HttpDelete("Delete")]
        public void Delete(int id)
        {
            var filtereddata = _redisCacheDbContext.TblEmployees.FirstOrDefault(x => x.Id == id);
            _redisCacheDbContext.TblEmployees.Remove(filtereddata);
            _redisCacheDbContext.SaveChanges();
            _cacheService.RemoveData("Employees");
        }

        [HttpPost("Insert")]
        public IActionResult Insert(TblEmployee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee data is null");
            }

            // Insert into database
            _redisCacheDbContext.TblEmployees.Add(employee);
            _redisCacheDbContext.SaveChanges();

            // Remove cached list so next GET fetches updated data
            _cacheService.RemoveData("Employees");

            // Optionally: you can set updated cache immediately to avoid cache miss next time
            var updatedEmployees = _redisCacheDbContext.TblEmployees.ToList();
            var expirationTime = DateTime.Now.AddMinutes(5);
            _cacheService.SetData<IEnumerable<TblEmployee>>("Employees", updatedEmployees, expirationTime);

            return Ok(employee);
        }

    }
}
