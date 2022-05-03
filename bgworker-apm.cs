public class Worker : BackgroundService {
	private readonly ILogger<Worker> _logger;
	private readonly ITracer _tracer;

	public Worker(ILogger<Worker> logger, ITracer tracer) {
		_logger = logger;
		_tracer = tracer;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		
		var transaction = _tracer.StartTransaction("My Transaction", ApiConstants.TypeRequest);
		
		while (!stoppingToken.IsCancellationRequested) {
			try {
				
				// var span11 = _tracer.CurrentTransaction?.StartSpan("step 1.1", "label 1.1");

				await transaction.CaptureSpan("step 1.1", "label 1.1",
						async () => {
							_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
							await Task.Delay(3500, stoppingToken);

							if (DateTime.Now.Second % 3 == 0) {
								throw new BooException("BooEx!");
							}
						});
			} catch (BooException e) {
				_logger.LogError("BooEx");
				transaction.CaptureException(e, "DateTime.Seconds is divisible by 3");
			}
			catch (Exception e) {
				transaction.CaptureException(e);
			}
			finally {
				transaction?.End();		
			}
		}
	}
}
