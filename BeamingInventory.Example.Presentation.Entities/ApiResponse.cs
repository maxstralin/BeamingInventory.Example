namespace BeamingInventory.Example.Presentation.Entities
{
    public class ApiResponse : ApiResponse<object>
    {
       
    }

    public class ApiResponse<T>
    {
        public bool Successful { get; set; }
        public string? Message { get; set; }
        //Here would be the return data from the api: could be prevCount vs newCount, etc.
        public T Data { get; set; } = default!;
    }
}
