using GameShow.Models;
using Microsoft.EntityFrameworkCore;

namespace GameShow.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Question> Questions => Set<Question>();
    }
}
