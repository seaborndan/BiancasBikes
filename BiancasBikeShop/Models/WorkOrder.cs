using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiancasBikeShop.Models
{
    public class WorkOrder
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateInitiated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public Bike Bike { get; set; }
    }
}
