# Utilities

This repository contains a collection of utility scripts and services to streamline development tasks. The primary focus is on providing reusable components that can be easily integrated into different projects.

## TaskQueueService

A robust task queue service to manage and throttle the rate of processing tasks. This service is useful for scenarios where you need to limit the number of requests sent to an external service over a given time period.

### Features

- **Rate Limiting:** Ensures that the number of tasks processed does not exceed the defined threshold within the specified period.
- **Queue Management:** Efficiently handles task queuing and dequeuing.
- **Concurrency Handling:** Manages concurrent task execution without missing any tasks.

### Usage

Here is an example of how to use the `TaskQueueService`:

```csharp
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        var taskQueueService = new TaskQueueService<string>(60000, 100); // periodDurationMs, maxRequestsPerPeriod

        for (int i = 0; i < 150; i++)
        {
            await taskQueueService.EnqueueRequest(async () => {
                await Task.Delay(100); // Simulate a task
                return "Task Completed";
            });
        }
    }
}
```

### Contributing
Feel free to submit pull requests to improve the codebase. Any contributions are welcome.

### License
This repository is licensed under the MIT License. See the [LICENSE](https://github.com/jasonred/Utilities/blob/main/LICENSE) file for more information.

Adapt it as you see fit! The example provided should give a good foundation.
