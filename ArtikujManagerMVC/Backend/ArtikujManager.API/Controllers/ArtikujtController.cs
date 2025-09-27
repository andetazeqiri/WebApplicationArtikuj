using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ArtikujManager.API.Data;
using ArtikujManager.API.DTOs;
using ArtikujManager.API.Models;

namespace ArtikujManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ArtikujtController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArtikujtController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       
        [HttpGet]
        [Authorize(Policy = "OperatorOrAdmin")]
        public async Task<ActionResult<IEnumerable<ArtikujDto>>> GetArtikujt([FromQuery] string? search = null)
        {
            var query = _context.Artikujt.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.Emri.Contains(search) || a.Barkodi.Contains(search));
            }

            var artikujt = await query
                .Select(a => new ArtikujDto
                {
                    Id = a.Id,
                    Emri = a.Emri,
                    Cmimi = a.Cmimi,
                    Njesia = a.Njesia,
                    Barkodi = a.Barkodi,
                    DataSkadences = a.DataSkadences,
                    Lloj = a.Lloj,
                    KaTvsh = a.KaTvsh,
                    Tipi = a.Tipi,
                    DataKrijimit = a.DataKrijimit,
                    DataModifikimit = a.DataModifikimit,
                    KrijuarNga = a.KrijuarNga,
                    ModifikuarNga = a.ModifikuarNga
                })
                .ToListAsync();

            return Ok(artikujt);
        }

        
        [HttpGet("{id}")]
        [Authorize(Policy = "OperatorOrAdmin")]
        public async Task<ActionResult<ArtikujDto>> GetArtikuj(int id)
        {
            var artikuj = await _context.Artikujt.FindAsync(id);

            if (artikuj == null)
            {
                return NotFound(new { message = "Artikulli nuk u gjet." });
            }

            var artikujDto = new ArtikujDto
            {
                Id = artikuj.Id,
                Emri = artikuj.Emri,
                Cmimi = artikuj.Cmimi,
                Njesia = artikuj.Njesia,
                Barkodi = artikuj.Barkodi,
                DataSkadences = artikuj.DataSkadences,
                Lloj = artikuj.Lloj,
                KaTvsh = artikuj.KaTvsh,
                Tipi = artikuj.Tipi,
                DataKrijimit = artikuj.DataKrijimit,
                DataModifikimit = artikuj.DataModifikimit,
                KrijuarNga = artikuj.KrijuarNga,
                ModifikuarNga = artikuj.ModifikuarNga
            };

            return Ok(artikujDto);
        }

        
        [HttpPost]
        [Authorize(Policy = "OperatorOrAdmin")] 
        public async Task<ActionResult<ArtikujDto>> CreateArtikuj([FromBody] CreateArtikujDto createArtikujDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId ?? "");
                var userName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown";

                var artikuj = new Artikuj
                {
                    Emri = createArtikujDto.Emri,
                    Cmimi = createArtikujDto.Cmimi,
                    Njesia = createArtikujDto.Njesia,
                    Barkodi = createArtikujDto.Barkodi,
                    DataSkadences = createArtikujDto.DataSkadences,
                    Lloj = createArtikujDto.Lloj,
                    KaTvsh = createArtikujDto.KaTvsh,
                    Tipi = createArtikujDto.Tipi,
                    DataKrijimit = DateTime.Now,
                    KrijuarNga = userName
                };

                _context.Artikujt.Add(artikuj);
                await _context.SaveChangesAsync();

                var artikujDto = new ArtikujDto
                {
                    Id = artikuj.Id,
                    Emri = artikuj.Emri,
                    Cmimi = artikuj.Cmimi,
                    Njesia = artikuj.Njesia,
                    Barkodi = artikuj.Barkodi,
                    DataSkadences = artikuj.DataSkadences,
                    Lloj = artikuj.Lloj,
                    KaTvsh = artikuj.KaTvsh,
                    Tipi = artikuj.Tipi,
                    DataKrijimit = artikuj.DataKrijimit,
                    DataModifikimit = artikuj.DataModifikimit,
                    KrijuarNga = artikuj.KrijuarNga,
                    ModifikuarNga = artikuj.ModifikuarNga
                };

                return CreatedAtAction(nameof(GetArtikuj), new { id = artikuj.Id }, artikujDto);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error creating artikuj: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                return StatusCode(500, new { message = "Ka ndodhur një gabim gjatë shtimit të artikullit.", error = ex.Message });
            }
        }

        
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")] 
        public async Task<IActionResult> UpdateArtikuj(int id, [FromBody] UpdateArtikujDto updateArtikujDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artikuj = await _context.Artikujt.FindAsync(id);
            if (artikuj == null)
            {
                return NotFound(new { message = "Artikulli nuk u gjet." });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId ?? "");
            var userName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown";

            artikuj.Emri = updateArtikujDto.Emri;
            artikuj.Cmimi = updateArtikujDto.Cmimi;
            artikuj.Njesia = updateArtikujDto.Njesia;
            artikuj.Barkodi = updateArtikujDto.Barkodi;
            artikuj.DataSkadences = updateArtikujDto.DataSkadences;
            artikuj.Lloj = updateArtikujDto.Lloj;
            artikuj.KaTvsh = updateArtikujDto.KaTvsh;
            artikuj.Tipi = updateArtikujDto.Tipi;
            artikuj.DataModifikimit = DateTime.Now;
            artikuj.ModifikuarNga = userName;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Artikulli u perditesua me sukses." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtikujExists(id))
                {
                    return NotFound(new { message = "Artikulli nuk u gjet." });
                }
                else
                {
                    throw;
                }
            }
        }

        
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")] 
        public async Task<IActionResult> DeleteArtikuj(int id)
        {
            var artikuj = await _context.Artikujt.FindAsync(id);
            if (artikuj == null)
            {
                return NotFound(new { message = "Artikulli nuk u gjet." });
            }

            _context.Artikujt.Remove(artikuj);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Artikulli u fshi me sukses." });
        }



        private bool ArtikujExists(int id)
        {
            return _context.Artikujt.Any(e => e.Id == id);
        }
    }
}