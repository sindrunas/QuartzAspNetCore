# Quartz Net Core - JobFactory Extension for ASP.NET CORE

## Introduction

This is the README file for Quartz NET CORE, .a job factory extension for Quartz.NET. It supports .NET Core/netstandard 2.2.

## How to use it

Firstly register the job factory in the startup.cs of your application inside the ConfigureServices method:

Ex: 	services.AddTransient<IJobFactory, QuartzJobFactory>(
		(provider) =>
		{
			return new QuartzJobFactory(provider);
		});
		
then you can use the jobfactory inside any class which receives the IServiceProvide thru DI:

Ex: 	var schedulerFactory = new StdSchedulerFactory();
		_scheduler = schedulerFactory.GetScheduler().Result;
		_scheduler.JobFactory = new QuartzJobFactory(_services);
		_scheduler.Start().Wait();
		
		await _scheduler.TriggerJob(new JobKey(taskScheduleId, jobGroupId));