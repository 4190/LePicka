using Microsoft.AspNetCore.Identity;

namespace Auth.Dto
{
    public class UserCreatedDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DataSourceMicroserviceName { get; set; }
        public string Event {  get; set; }
    }
}
