using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HalliburtonTest
{
    public class Trip
    {

        public int Id { get; set; }

        public DateTime TripDate { get; set; }


        public EnumTripType TripType { get; set; }

        public int TripDays { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public enum EnumTripType
        {
            Boarding = 1,
            Landing = 2
        }

    }
}
