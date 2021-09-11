using System.Collections.Generic;

namespace BeamingInventory.Example.Presentation.Entities.Interfaces
{
    public interface IConfiguration
    {
        string ApiUrl { get; }
        IReadOnlyDictionary<char, string> Endpoints { get; }
    }
}