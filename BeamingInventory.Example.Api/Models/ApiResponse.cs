namespace BeamingInventory.Example.Api.Models
{
    public class ApiResponse : ApiResponse<object>
    {
       
    }

    public class ApiResponse<T>
    {
        public ApiResponse()
        {
            
        }

        public ApiResponse(bool successful, T data)
        {
            Successful = successful;
            Data = data;
        }
        public bool Successful { get; set; }
        public string? Message { get; set; }
        public T Data { get; set; } = default!;
    }
}
