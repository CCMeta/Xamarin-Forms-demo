using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Xamarin_Forms_demo.Services
{
    public class AppConfiguration : ConfigurationBuilder
    {

        public static IConfiguration GetInstence()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resName = assembly.GetManifestResourceNames()?.
                FirstOrDefault(r => r.EndsWith("AppSettings.json", StringComparison.OrdinalIgnoreCase));
            var file = assembly.GetManifestResourceStream(resName);
            return new AppConfiguration().AddJsonStream(file).Build();
        }
    }
}
