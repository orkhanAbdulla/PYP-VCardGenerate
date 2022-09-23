using Microsoft.EntityFrameworkCore;
using PYP_QrCodeGeneration.Models;

namespace PYP_QrCodeGeneration.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}
        public DbSet<VCard> VCards { get; set; }
    }
}
