using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Psns.Common.Logging
{
    public interface ILogger : IDisposable
    {
        void Write(LoggerEntry entry);
    }
}
