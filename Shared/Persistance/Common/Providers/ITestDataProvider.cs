namespace Shared.Persistance.Common.Providers
{
    public interface ITestDataProvider
    {
        Task GenerateTestDataAsync();
        Task<bool> HasTestDataAsync();
        Task ClearTestDataAsync();
    }
} 