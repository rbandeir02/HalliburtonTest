using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HalliburtonTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApiContext _context;

        public EmployeeController(ApiContext context)
        {
            _context = context;
        }
        // GET: api/<EmployeeController>
        [HttpGet("GetAllEmployees")]
        public IEnumerable<Employee> GetAllEmployees()
        {
            IQueryable<Employee> resposta;
            List<Employee> r;
            var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase(databaseName: "Test").Options;

            using (var context = new ApiContext(options))
            {

                var oEmployees = context.Employees;

                resposta = oEmployees.Select(u => new Employee
                {
                    EmployeeId = u.EmployeeId,
                    Nome = u.Nome,
                    Funcao = u.Funcao,
                    Empresa = u.Empresa
                    //  Trips = u.Trips.Select(p => p)
                });
                r = resposta.ToList();
            }
            // return Ok(resposta);

            return r;
        }

        [HttpGet("GetAllEmployeeByTripDate")]
        public IEnumerable<Trip> GetAllEmployeeByTripDate(DateTime embarque, DateTime desembarque)
        {
            IQueryable<Trip> resposta;
            List<Trip> r;
            var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase(databaseName: "Test").Options;

            using (var context = new ApiContext(options))
            {

                var oTrip = context.Trips;
                var oEmployees = context.Employees;

                resposta = oTrip.Where(c => c.TripDate > embarque)
                    .Where(c => c.TripType == Trip.EnumTripType.Boarding);
                resposta = resposta.Where(c => c.TripDate < desembarque)
                    .Where(c => c.TripType == Trip.EnumTripType.Landing);

                //resposta = oTrip.Where(c => c.TripDate == embarque)
                //    .Select((u => new Trip
                //    {
                //        TripDate = u.TripDate,
                //        EmployeeId = u.EmployeeId,
                //        TripType = u.TripType
                //        //  Trips = u.Trips.Select(p => p)
                //    }));
                r = resposta.ToList();

                foreach (Trip trip in r)
                {
                    trip.Employee = oEmployees.Find(trip.EmployeeId);
                }
            }
            // return Ok(resposta);

            return r;
        }



        // POST api/<EmployeeController>
        [HttpPost("New")]
        public ContentResult Post(string nome, string funcao, string empresa)
        {
            //TODO: add validation
            // _context.Add(oEmployee);
            //_context.SaveChanges();

            Employee oEmployee;
            var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase(databaseName: "Test").Options;

            using (var context = new ApiContext(options))
            {

                oEmployee = new Employee();
                oEmployee.Nome = nome;
                oEmployee.Empresa = empresa;
                oEmployee.Funcao = funcao;
                context.Employees.Add(oEmployee);
                context.SaveChanges();

            }

            return new ContentResult() { StatusCode = 200, Content = "Empregado número " + oEmployee.EmployeeId.ToString() + "  registrado. " };
            //return Ok;
        }

        [HttpPost("Boarding")]
        public ContentResult Boarding(int EmployeeId, DateTime TripDate)
        {
            //TODO: add validation
            // _context.Add(oEmployee);
            //_context.SaveChanges();

            if (TripDate > DateTime.Now.AddDays(15))
            {
                return new ContentResult() { StatusCode = 428, Content="Máximo de 15 dias da data de embarque do dia atual." };
            }


            Trip oTrip;
            var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase(databaseName: "Test").Options;

            using (var context = new ApiContext(options))
            {

                oTrip = new Trip();
                oTrip.TripDate = TripDate;
                oTrip.TripType = Trip.EnumTripType.Boarding;
                oTrip.EmployeeId = EmployeeId;
                context.Trips.Add(oTrip);
                context.SaveChanges();

            }

            return new ContentResult() { StatusCode = 200, Content = "Embarque número "+ oTrip.Id.ToString() +" registrado para o dia " + TripDate.ToString("dd/MM/yyyy hh:mm:ss") + ". "};
            }


        [HttpPost("Landing")]
        public ContentResult Landing(int EmployeeId, DateTime TripDate, int BoardingId)
        {
            //TODO: add validation
            // _context.Add(oEmployee);
            //_context.SaveChanges();

            Trip oTrip;
            Trip oBoarding;
            var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase(databaseName: "Test").Options;

            using (var context = new ApiContext(options))
            {
                oBoarding = context.Trips.Find(BoardingId);

                if (oBoarding == null)
                {
                    return new ContentResult() { StatusCode = 428, Content = "Embarque não encontrado." };
                }
                if (TripDate < oBoarding.TripDate)
                {
                    return new ContentResult() { StatusCode = 428, Content = "Desembarque não pode ser antes do embarque." };
                }
                if ((TripDate - oBoarding.TripDate).TotalDays < 7)
                {
                    return new ContentResult() { StatusCode = 428, Content = "Mínimo de 7 dias de folga entre embarque e desembarque." };
                }

                oTrip = new Trip();
                oTrip.TripDate = TripDate;
                oTrip.EmployeeId = EmployeeId;
                oTrip.TripType = Trip.EnumTripType.Landing;
                context.Trips.Add(oTrip);
                context.SaveChanges();

            }

            return new ContentResult() { StatusCode = 200, Content = "Desemmbarque número "+ oTrip.Id.ToString() +" registrado para o dia " + TripDate.ToString("dd/MM/yyyy hh:mm:ss") + ". " };
        }

        
    }
}
