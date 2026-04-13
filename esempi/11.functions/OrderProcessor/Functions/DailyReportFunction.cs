// Functions/DailyReportFunction.cs
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace OrderProcessor.Functions;

public class DailyReportFunction
{
    private readonly ILogger<DailyReportFunction> _logger;

    public DailyReportFunction(ILogger<DailyReportFunction> logger)
    {
        _logger = logger;
    }

    // TimerTrigger: ogni giorno alle 02:00 UTC
    // cron: secondo minuto ora giorno mese giorno-della-settimana
    [Function("DailyReport")]
    public void Run([TimerTrigger("0 0 2 * * *")] TimerInfo myTimer)
    {
        if (myTimer.IsPastDue)
        {
            _logger.LogWarning("Il timer è in ritardo; l'ultima esecuzione avrebbe dovuto avvenire prima.");
        }

        _logger.LogInformation("Generazione report giornaliero ordini — {Time:O}", DateTime.UtcNow);

        // Logica di generazione report (stub per il corso)
    }
}
