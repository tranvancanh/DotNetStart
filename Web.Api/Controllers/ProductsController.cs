using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tozan.Server.ConnectionString;
using WebApi.Commons;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductsController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: <ProductsController>
        [HttpGet]
        [Route("products")]
        public async Task<JsonResult> Product()
        {
            var data = new List<Product>();
            using (var connection = new SqlConnection(GetConnectString.GetInstance().ConnectionString))
            {
                try
                {
                    string selectString = $@"
                                          SELECT TOP (2000)
	                                               [Id]
                                                  ,[Name]
                                                  ,[Price]
                                                  ,[Status]
                                              FROM [Products]
                                            order by id asc
                                        ";
                    data = (await connection.QueryAsync<Product>(selectString)).ToList();
                }
                catch (Exception ex)
                {
                    return new JsonResult(null)
                    {
                        StatusCode = StatusCodes.Status500InternalServerError, // Status code here 
                        Value = ex.Message,
                    };
                    
                }
            }
            if (!(data is null))
            {
                return new JsonResult(data)
                {
                    StatusCode = StatusCodes.Status200OK, // Status code here 
                    Value = data,
                };
            }
            else
            {
                return new JsonResult(data)
                {
                    StatusCode = StatusCodes.Status404NotFound, // Status code here 
                    Value = data,
                };
            }
        }

        // GET <ProductsController>/5
        [HttpGet]
        [Route("getproduct")]
        public async Task<ActionResult> GetProduct(int Id)
        {
            var data = new Product();
            using (var connection = new SqlConnection(GetConnectString.GetInstance().ConnectionString))
            {
                try
                {
                    string selectString = $@"
                                          SELECT TOP (10)
	                                               [Id]
                                                  ,[Name]
                                                  ,[Price]
                                                  ,[Status]
                                              FROM [Products]
                                              WHERE [Id]=@Id;
                                        ";
                    data = await connection.QueryFirstAsync<Product>(selectString, new {Id = Id});
                }
                catch (Exception ex)
                {
                    //log error
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status200OK, data);
        }

        // POST <ProductsController>
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult> ProductAdd(Product product)
        {

            string commandText = $@"
                                    INSERT INTO [Products] (Id, Name, Price, Status)
                                    VALUES (@Id, @Name, @Price, @Status);
                                        ";
            var affectedRows = -1;
            using (var con = new SqlConnection(GetConnectString.GetInstance().ConnectionString))
            {
                affectedRows = await con.ExecuteAsync(commandText, new
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Status = product.Status
                });
            }
            if (affectedRows > 0)
            {
                var data = new List<Product>();
                using (var connection = new SqlConnection(GetConnectString.GetInstance().ConnectionString))
                {
                    try
                    {
                        string selectString = $@"
                                          SELECT TOP (2000)
	                                               [Id]
                                                  ,[Name]
                                                  ,[Price]
                                                  ,[Status]
                                              FROM [Products]
                                            order by id asc
                                        ";
                        data = (await connection.QueryAsync<Product>(selectString)).ToList();
                    }
                    catch (Exception ex)
                    {
                        //log error
                        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                    }
                }
                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return BadRequest();
            }
        }
    



        // PUT <ProductsController>/5
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(Product product)
        {
            var affectedRows = -1;
            using (var connection = new SqlConnection(GetConnectString.GetInstance().ConnectionString))
            {
                try
                {
                    string commandText = $@"
                                        IF(EXISTS (SELECT * FROM [Products] WHERE [Id] = @Id ))
                                        BEGIN
                                            UPDATE [Products]
                                            SET [Id] = @Id, [Name] = @Name, [Price] = @Price, [Status] = @Status
                                            WHERE [Id] = @Id;
                                        END
                                        ELSE
                                        BEGIN
                                            INSERT INTO [Products]
                                                   ([Id], [Name], [Price], [Status])
                                            VALUES (@Id,  @Name,  @Price,  @Status);
                                        END
                                        ";
                    affectedRows = await connection.ExecuteAsync(commandText, new
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Status = product.Status,
                    });
                }
                catch (Exception ex)
                {
                    //log error
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            if(affectedRows > 0)
            {
                return StatusCode(StatusCodes.Status200OK, product);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, null);
            }
        }

        // DELETE <ProductsController>/5
        [HttpGet]
        [Route("delete")]
        public async Task<JsonResult> Delete(int Id)
        {
            var a1 = new ServerPath(_hostingEnvironment)._Server_Path;
            var a = _hostingEnvironment.EnvironmentName;
            string commandText = $@"
                                    DELETE FROM [Products] WHERE [Id]=@Id;
                                    ";
            var affectedRows = -1;
            try
            {
                using (var con = new SqlConnection(GetConnectString.GetInstance().ConnectionString))
                {
                    affectedRows = await con.ExecuteAsync(commandText, new
                    {
                        Id = Id,
                    });
                }
            }
            catch(Exception ex)
            {
                //log error
                return new JsonResult(null)
                {
                    StatusCode = StatusCodes.Status500InternalServerError, // Status code here 
                    Value = ex.Message,
                };
            }
            if(affectedRows > 0)
            {
                return new JsonResult(Id)
                {
                    StatusCode = StatusCodes.Status200OK, // Status code here 
                    Value = Id,
                };
            }
            else
            {
                return new JsonResult(Id)
                {
                    StatusCode = StatusCodes.Status400BadRequest, // Status code here 
                    Value = Id,
                };
                
            }
        }

        [HttpGet]
        [Route("createproductid")]
        public async Task<ActionResult> CreateProductId()
        {
            var data = new Product();
            var nextProductId = 0;
            using (var connection = new SqlConnection(GetConnectString.GetInstance().ConnectionString))
            {
                try
                {
                    string selectString = $@"
                                          DECLARE @Id_Table TABLE (
                                            Id int NOT NULL 
                                           );

                                          DECLARE @ii INT;
                                          DECLARE @Id_Max INT;
                                          SET @ii = 1;
                                          SET @Id_Max = (select MAX([Id]) FROM [Products]) + 1;
                                          WHILE @ii <= @Id_Max 
                                            BEGIN
                                              INSERT INTO @Id_Table
                                              VALUES (@ii)
                                              SET @ii = @ii + 1;
                                            END
  
                                           SELECT  * From  @Id_Table
                                            where Id not in (SELECT DISTINCT [Id]
                                            FROM [Products])
	                                        order by Id asc
                                        ";
                    data = await connection.QueryFirstAsync<Product>(selectString);
                }
                catch (Exception ex)
                {
                    //log error
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            if(data is not null)
            {
                nextProductId = data.Id;
            }
            return StatusCode(StatusCodes.Status200OK, nextProductId);
        }
    }
}
