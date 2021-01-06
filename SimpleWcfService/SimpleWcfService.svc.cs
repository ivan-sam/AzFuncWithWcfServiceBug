using System;

namespace SimpleWcfService
{
    public class SimpleWcfService : ISimpleWcfService
    {
        public string GetData()
        {
            return DateTime.UtcNow.ToString("o");
        }
    }
}
