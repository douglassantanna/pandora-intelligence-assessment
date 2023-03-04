using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Persistance;
public class Datacontext : DbContext
{
    public Datacontext(DbContextOptions<Datacontext> options) : base(options)
    {
    }
    public DbSet<Car> Cars { get; set; } = null!;
}