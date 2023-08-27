using System.Data;
using WebApi.Models.Users;

namespace WebApi.DataAccess.Login
{
    public class LoginAcc
    {

        public async Task<DataTable> GetUser(UserLogin userLogin)
        {
            var data = new DataTable();
            var SQL_SELECT = $@"
                                SELECT *
                                FROM [Users]
                                WHERE (1=1)
                                AND [UserName] = @UserName
                                AND [Password] = @Password
                            ";
            using (var sqlAccess = new Tozan.Server.SQLAccess.SQLAccess())
            {
                data = await sqlAccess.ExecQueryDataTableAsync(SQL_SELECT, new
                {
                    UserName = userLogin.UserName,
                    Password = userLogin.Password
                });
            }

            return data;
        }
    }
}
