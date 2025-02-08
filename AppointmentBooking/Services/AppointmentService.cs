using AppointmentBooking.Data;
using AppointmentBooking.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppointmentBooking.Services
{
    /// <summary>
    /// Service for managing appointment bookings and fetching available slots.
    /// </summary>
    public class AppointmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AppointmentService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public AppointmentService(ApplicationDbContext context, ILogger<AppointmentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<AvailableSlot>> GetAvailableSlots(SearchCriteria criteria)
        {
            try
            {
                var requiredLanguage = new[] { criteria.Language };
                //Find Sales Managers matching criteria
                var matchingManagers = await _context.SalesManagers
                    .Where(sm => sm.Languages.Contains(criteria.Language) &&
                                sm.Products.Intersect(criteria.Products).Count() == criteria.Products.Length &&
                                sm.CustomerRatings.Contains(criteria.Rating))
                    .Select(sm => sm.Id)
                    .ToListAsync();

                if (!matchingManagers.Any())
                {
                    _logger.LogInformation("No matching sales managers found for the given criteria.");
                    return new List<AvailableSlot>();
                }

                var requestedTimeUtc = DateTime.SpecifyKind(criteria.Date.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

                var groupedSlots = await _context.Slots
                .Where(s => !s.Booked &&
                            matchingManagers.Contains(s.SalesManagerId) &&
                            s.StartDate.Date == requestedTimeUtc &&
                            !_context.Slots.Any(bs => bs.SalesManagerId == s.SalesManagerId &&
                                                      bs.Booked &&
                                                      bs.StartDate < s.EndDate &&
                                                      bs.EndDate > s.StartDate))
                .GroupBy(s => s.StartDate)
                .Select(g => new AvailableSlot
                {
                    StartDate = g.Key,
                    AvailableCount = g.Count()
                })
                .OrderBy(g => g.StartDate)
                .ToListAsync();

                return groupedSlots;
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Input validation error: {ex.Message}");
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Database error: {dbEx}");
                throw new Exception("A database error occurred. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex}");
                throw;
            }
        }
    }
}
