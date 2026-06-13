using Shared.Persistance.Common.Options;

namespace Shared.Persistance.Common.Providers
{
    public interface IDatabaseOptionsProvider
    {
        DatabaseOptions Value { get; }
    }
} 