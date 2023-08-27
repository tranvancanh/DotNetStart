using Dapper;
using JWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Tozan.Server.ConnectionString;
using WebApi.Models;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerCommon
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        // GET: EmployeeController
        [HttpGet]
        [Route("Index")]
        public JsonResult Index(string search)
        {
            var response = new List<Employee>();
            using (var connection = new SqlConnection(GetConnectString.GetInstance().ConnectionString))
            {
                connection.Open();
                string selectString = $@"SELECT TOP(@Limit) *
                                            FROM [employee].[dbo].[m_employee]
                                            Where id_employee like @Search OR name_employee like @Search
                                            order by id_employee asc";
                response = connection.Query<Employee>(selectString, new { Limit = 100, Search = "%" + search + "%" }).ToList();

            }
            return new JsonResult(response);
        }

        // GET: EmployeeController
        [Authorize(Roles = "admin, user")]
        [HttpPost]
        [Route("alluser")]
        public async Task<JsonResult> GetAllUsers(string search)
        {
            //var un = CurrentUserName;
            string userName = User.Claims.First(x => x.Type == "UserName").Value;
            var header = Request.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));
            string HeaderKeyName = "FilterHeaderKey";
            var a = HttpContext.Items.TryGetValue(HeaderKeyName, out object filterHeaderValue);

            var response = new List<User>();
            using (var connection = new SqlConnection(GetConnectString.GetInstance().ConnectionString))
            {
                connection.Open();
                string selectString = $@"SELECT * 
                                          FROM [Users]";
                response = (await connection.QueryAsync<User>(selectString)).ToList();

            }

            //DataTable dt = new DataTable();
            //var dtColumn = new DataColumn();
            //dt.Columns.Add(dtColumn);
            //dt.Columns.Add(dtColumn);
            //object[] o = { "Ravi", 500 };
            //dt.Rows.Add(o);
            //dt.Rows.Add(new object[] { "Ravi", 500 });

            //var data = FileExcelHandle.ReadFileExcelWithSingleSheet("\\\\nas1\\d\\data\\email\\KUNO\\納入指示0201.xlsx");
            //await Task.Delay(5000);
            //var model = new Response<Employee>(response);
            return new JsonResult(response);
        }


        // GET: EmployeeController/Details/5
        [Route("Details")]
        public ActionResult Details(string id)
        {
            int a = Convert.ToInt32(id);
            var em = new[] { "13", "456", "789" };
            var listString = string.Join(string.Empty, "123", "456");
            return new JsonResult(listString);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return new JsonResult(result);
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return new JsonResult("");
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return new JsonResult("");
            }
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return new JsonResult("");
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            return new JsonResult("");
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var listObj = new List<object>();
                var anonymousObject = new { Name = "Anonymous", Value = "Foo" };
                listObj.Add(anonymousObject);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return new JsonResult("");
            }
        }
    }


}
