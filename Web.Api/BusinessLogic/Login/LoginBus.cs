using System.Data;
using WebApi.DataAccess.Login;
using WebApi.Models.Users;

namespace WebApi.BusinessLogic.Login
{
    public class LoginBus
    {
        private readonly LoginAcc loginAcc = null;

        public LoginBus()
        {
            if(loginAcc is null)
            {
                loginAcc = new LoginAcc();
            }
        }

        public async Task<DataTable> GetUser(UserLogin userLogin)
        {

            var user = await loginAcc.GetUser(userLogin);
            if(user.Rows.Count > 0)
            {
                return user;
                //var userName = Convert.ToString(user.Rows[0]["UserName"]);
                //var password = Convert.ToString(user.Rows[0]["Password"]);
                //return new UserLogin()
                //{
                //    UserName = userName,
                //    Password = password,
                //};
            }
            else
            {
                return null;
            }
           
        }
    }
}
