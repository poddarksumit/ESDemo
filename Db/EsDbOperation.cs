using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESDemo.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace ESDemo.Db
{
    public class EsDbOperation : IEsDbOperation
    {
        public static EsDbContext Context = new EsDbContext();
        public List<KcaCustomer> GetCustomers()
        {
            return Context.KcaCustomer.Include(i => i.School).Include(i => i.School.Status).Include(i => i.School.Suburb).Include(i => i.School.Suburb.Postcode).Include(i => i.School.Suburb.Postcode.State).ToList();
        }

        public int GetCustomersCount()
        {
            return Context.KcaCustomer.Count();
        }

        public List<KcaCustomer> GetCustomers(int size, int takeNumber)
        {
            return Context.KcaCustomer
                .Include(i => i.School)
                .Include(i => i.School.Status)
                .Include(i => i.School.Suburb)
                .Include(i => i.School.Suburb.Postcode)
                .Include(i => i.School.Suburb.Postcode.State)
                .OrderBy(o => o.Id)
                .Skip(takeNumber).Take(size + takeNumber)
                .ToList();
        }

        public KcaCustomer GetDbCustomer(string email)
        {
            return Context.KcaCustomer.Include(i => i.School).Include(i => i.School.Status).Include(i => i.School.Suburb).Include(i => i.School.Suburb.Postcode).Include(i => i.School.Suburb.Postcode.State).Where(x => x.Email.Equals(email)).FirstOrDefault();
        }
    }
}
