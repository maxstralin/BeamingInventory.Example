using System.ComponentModel.DataAnnotations;

namespace BeamingInventory.Example.Api.ViewModels
{
    public class PostSaleVm
    {
        [Range(1, int.MaxValue, ErrorMessage = "Count must be 1 or more")]
        public int Count { get; set; }
    }
}
