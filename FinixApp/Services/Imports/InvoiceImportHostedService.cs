namespace backend.Services.Imports
{
    using backend.Interfaces;

    public class InvoiceImporterHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public InvoiceImporterHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var importer = scope.ServiceProvider.GetRequiredService<IInvoiceImport>();
            await importer.ImportAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
