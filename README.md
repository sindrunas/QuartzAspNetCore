## How to use it

Firstly register the job factory in the startup.cs of your application inside the ConfigureServices method:

```csharp
services.AddTransient<IJobFactory, QuartzJobFactory>(
		(provider) =>
		{
			return new QuartzJobFactory(provider);
		});
```

then you can use the jobfactory inside any class which receives the IServiceProvide thru DI:

```csharp
var schedulerFactory = new StdSchedulerFactory();
_scheduler = schedulerFactory.GetScheduler().Result;
_scheduler.JobFactory = new QuartzJobFactory(_services);
_scheduler.Start().Wait();
		
await _scheduler.TriggerJob(new JobKey(taskScheduleId, jobGroupId));
```