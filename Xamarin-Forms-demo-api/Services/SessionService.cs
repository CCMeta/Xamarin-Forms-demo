using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Services
{
    public class SessionService : IDisposable
    {
        public Dictionary<string, int> sessions = new Dictionary<string, int>();

        public Dictionary<string, int> Sessions
        {
            get { return sessions; }
            set
            {
                sessions = value;
            }
        }

        public SessionService()
        {
            Sessions.Add("a", 1);
            Sessions.Add("b", 2);
            Sessions.Add("c", 3);
            Sessions.Add("d", 4);
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
