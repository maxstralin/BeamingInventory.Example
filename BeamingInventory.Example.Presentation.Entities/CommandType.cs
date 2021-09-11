using System;

namespace BeamingInventory.Example.Presentation.Entities
{
    //This could have of course allowed for an array of types for multiple types of params but felt overkill for this example.
    public class CommandType
    {
        /// <summary>
        /// Command without param
        /// </summary>
        /// <param name="commandChar">The character to input to trigger command</param>
        public CommandType(char commandChar)
        {
            CommandChar = commandChar;
        }
        /// <summary>
        /// Command with a param
        /// </summary>
        /// <param name="commandChar">The character to input to trigger command</param>
        /// <param name="paramType">The type of the param</param>
        public CommandType(char commandChar, Type? paramType) : this(commandChar)
        {
            ParamType = paramType;
        }

        public char CommandChar { get; }
        public string? Description { get; set; }
        public Type? ParamType { get; }
    }
}
