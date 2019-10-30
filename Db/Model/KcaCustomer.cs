using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ESDemo.Db.Model
{
    public class KcaCustomer
    {
        public KcaCustomer()
        {

        }
        [Key]
        public int Id { get; set; }
        public int Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Landline { get; set; }
        public KcaSchool School { get; set; }
    }
}
