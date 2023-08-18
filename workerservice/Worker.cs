namespace workerservice
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        public Worker(ILogger<Worker> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                string Filelocation = _configuration.GetValue<string>("Filepath");
                string Filename = _configuration.GetValue<string>("Filename");
                Createfile(Filelocation, Filename);
                await Task.Delay(5000);
                Deletefile(Filelocation);
                await Task.Delay(5000, stoppingToken);
            }
        }

        private void Deletefile(string Filelocation)
        {
            try
            {
                string[] Files = Directory.GetFiles(Filelocation);
                foreach (var file in Files)
                {
                    File.Delete(file);
                    _logger.LogInformation("File deleted successfully:{file}", file,DateTime.Now);
                }
            }
            catch(Exception exc) {
                _logger.LogInformation(exc.Message);
            
            }
        }

        private void Createfile(string Filelocation, string Filename)
        {
            var file = Filelocation + "\\" + Filename;
            if (!File.Exists(file))
            {
                for (int i = 0; i <= 100; i++)
                {
                    var Fname = Filelocation + "\\" + "Workerfile_" + i + ".txt";
                    File.Create(Fname);
                    _logger.LogInformation("File created:{Fname} successfully", Fname);
                }
            }
        }
        }
}