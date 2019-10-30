using ESDemo.Db.Model;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace ESDemo.Db
{
    public class EsDbContext : DbContext
    {
        public virtual Microsoft.EntityFrameworkCore.DbSet<KcaCustomer> KcaCustomer { get;set;}
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=tcp:kc-ubykotex-portal.database.windows.net,1433;Database=kc-ubykotex-portal;User ID=ubykotex@kc-ubykotex-portal;Password='EgS2VVR6sDbVHzNy'");
        }
    }
}
