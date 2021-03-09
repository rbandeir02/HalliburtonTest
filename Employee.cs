using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HalliburtonTest
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string Nome { get; set; }

        public string Funcao { get; set; }

        public string Empresa { get; set; }

        public virtual List<Trip> Trips { get; set; }

    }
}
