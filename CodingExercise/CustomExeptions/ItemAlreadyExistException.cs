using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingExercise.CustomExeptions
{
    public class ItemAlreadyExistException : Exception
    {
        public ItemAlreadyExistException(string key) : base($"Item with key: {key} already exist")
        {

        }
    }
}
