namespace LePickaOrders.Application.Dtos.EventDtos
{
    public class AuthUserAddedDto
    {
        public string Id { get; set; }   //original microservice's entity ID
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DataSourceMicroserviceName { get; set; }
        public string Event { get; set; }
    }
}
