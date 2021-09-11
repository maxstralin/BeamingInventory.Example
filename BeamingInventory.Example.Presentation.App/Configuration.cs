using System;
using System.Collections.Generic;
using BeamingInventory.Example.Presentation.Entities;
using BeamingInventory.Example.Presentation.Entities.Interfaces;

namespace BeamingInventory.Example.Presentation.App
{
    //This could have been a JSON file, db table, etc.
    public class Configuration : IConfiguration
    {
        public Configuration(string? apiUrl = null)
        {
            ApiUrl = apiUrl ?? Environment.GetEnvironmentVariable("ApiUrl") ?? throw new NullReferenceException("No api url provided");
        }

        public string ApiUrl { get; }
        public IReadOnlyDictionary<char, string> Endpoints { get; } = new Dictionary<char, string>
        {
            ['S'] = "/sales",
            ['I'] = "/inventory",
            ['L'] = "/inventory"
        };

    }
}
