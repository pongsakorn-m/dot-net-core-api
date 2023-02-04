namespace DotNetCoreApi.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int FailCount { get; set; }
        public DateTime? LockoutEnd { get; set; }
    }
}
