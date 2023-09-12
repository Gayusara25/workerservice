using Microsoft.Extensions.Configuration;

public class BackgroundWorkerService : BackgroundService
{
    private readonly ILogger<BackgroundWorkerService> _logger;
    private readonly IConfiguration _configuration;
    private readonly TimeSpan _period = TimeSpan.FromSeconds(5);
    public string Filelocation;
    public string Filename;


    public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        Filelocation = _configuration.GetValue<string>("Filepath");
        Filename = _configuration.GetValue<string>("Filename");
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {          

            Createfile(Filelocation, Filename);
            _logger.LogInformation(DateTime.Now.ToString());           

        }
    }
    private void Createfile(string Filelocation, string Filename)
    {
        var Fname = Filelocation + "\\" + Filename;      

        string text = "File created " + DateTime.Now.ToString() + " " + "\n";
        File.AppendAllText(Fname, text);
        _logger.LogInformation(Fname);
     

    }


    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service started.");
        var Fname = Filelocation + "\\" + Filename;
        string text = "Service started" + "\n";
        File.AppendAllText(Fname, text);
         return base.StartAsync(cancellationToken);

    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service stopped.");
        var Fname = Filelocation + "\\" + Filename;
        string text = "Service stopped";
        File.AppendAllText(Fname, text);
        return base.StopAsync(cancellationToken);


    }
}