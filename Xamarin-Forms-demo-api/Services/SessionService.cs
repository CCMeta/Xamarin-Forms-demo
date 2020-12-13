using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public string CreatToken(string uid)
        {
            byte[] salt = new byte[16];
            RandomNumberGenerator.Create().GetBytes(salt);
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: uid,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32));
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
