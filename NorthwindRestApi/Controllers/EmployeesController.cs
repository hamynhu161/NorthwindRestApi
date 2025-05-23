﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public EmployeesController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound("Työntekijää ei löytynyt id:llä" + id);
            }

            return employee;
        }

        // Hakee työntenkijä maan nimen mukaan
        [HttpGet("Country/{countryname}")]
        public async Task<ActionResult<Employee>> GetEmployeeByCountry(string countryname)
        {
            var employees = await _context.Employees.Where(e => e.Country == "Finland").ToListAsync();

            if (employees == null)
            {
                return NotFound("Työntekijää ei löytynyt maan nimellä " + countryname);
            }

            return Ok(employees);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromBody]Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest($"Tapahtui virhe. id {id} ei ole olemassa");
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound($"{id} ei ole olemassa.");
                }
                else
                {
                    return StatusCode(500, "Yritä uudelleen.");
                }
            }

            return Ok($"Työntekijä päivitetty onnistuneesti id:llä {id}.");
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee([FromBody]Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return Ok($"Lisättiin uusi työntekijä {employee.EmployeeId}");
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException?.Message;
                return StatusCode(500, new { message = "Database update failed", details = innerException });
            }

        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound("Työntekijää ei löytynyt.");
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok("Tuote poistettiin.");
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
