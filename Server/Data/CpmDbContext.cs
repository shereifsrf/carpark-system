using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Server.Data;

public class CpmDbContext: DbContext
{
    public CpmDbContext(DbContextOptions<CpmDbContext> options) : base(options)
    {
    }

    public DbSet<Entry> Entries { get; set; } = null!;
}