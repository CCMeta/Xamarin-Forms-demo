using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Services
{
    public class SessionService : IDisposable
    {
        public Dictionary<string, int> sessions = new Dictionary<string, int>();

        public SessionService()
        {
            sessions.Add("a", 1);
            sessions.Add("b", 2);
            sessions.Add("c", 3);
            sessions.Add("d", 4);
        }

        public void Write(string message)
        {
            Console.WriteLine($"Service1: {message}");
        }

        private bool _disposed;
        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
        }
    }
}
