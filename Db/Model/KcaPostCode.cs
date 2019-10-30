using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ESDemo.Db.Model
{
    public class KcaPostCode
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public KcaState State { get; set; }
    }
}
