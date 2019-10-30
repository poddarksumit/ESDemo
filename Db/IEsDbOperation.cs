using ESDemo.Db.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESDemo.Db
{
    public interface IEsDbOperation
    {
        List<KcaCustomer> GetCustomers();
        KcaCustomer GetDbCustomer(string email);
        int GetCustomersCount();
        List<KcaCustomer> GetCustomers(int size, int takeNumber);
    }
}
