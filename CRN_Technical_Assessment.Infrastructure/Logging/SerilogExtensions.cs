using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Infrastructure.Logging
{
    public static class SerilogExtensions 
    { 
        public static void ConfigureSerilog(
            this IHostBuilder hostBuilder) 
        { hostBuilder.UseSerilog((context, config) => 
        {
            config
             .WriteTo.
             Console()
             .WriteTo
             .File(
                "logs/log-.txt",
                rollingInterval: RollingInterval.Day
                  )
             .ReadFrom.Configuration
             (
                context.Configuration
             ); 
        }
        ); 
        } 
    }
}
