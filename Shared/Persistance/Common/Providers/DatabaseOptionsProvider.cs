using Microsoft.Extensions.Options;
using Shared.Persistance.Common.Options;

namespace Shared.Persistance.Common.Providers
{
    public class DatabaseOptionsProvider : IDatabaseOptionsProvider
    {
        public DatabaseOptions Value { get; }

        public DatabaseOptionsProvider(IOptions<DatabaseOptions> options)
        {
            Value = options.Value;
        }
    }
} 