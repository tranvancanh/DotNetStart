namespace WebApi.Models.Users
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
