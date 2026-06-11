using SpinaBets.Models;
using Microsoft.EntityFrameworkCore;
using SpinaBets.Services.Interfaces;
using SpinaBets.DbContext;


namespace SpinaBets.Services
{
    public class SportService : ISportService
    {
        private readonly ApplicationDbContext _context;

        public SportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sport>> GetAllAsync()
        {
            return await _context.Sports
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Sport?> GetByIdAsync(int id)
        {
            return await _context.Sports.FindAsync(id);
        }

        public async Task CreateAsync(Sport sport)
        {
            _context.Sports.Add(sport);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sport sport)
        {
            var existing = await _context.Sports.FindAsync(sport.SportId);

            if (existing == null)
                throw new Exception("Sport not found.");

            existing.Name = sport.Name;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sport = await _context.Sports.FindAsync(id);

            if (sport != null)
            {
                _context.Sports.Remove(sport);
                await _context.SaveChangesAsync();
            }
        }
    }
}
