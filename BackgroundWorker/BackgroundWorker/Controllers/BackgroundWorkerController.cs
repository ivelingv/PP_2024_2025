using BackgroundWorker.Models;
using BackgroundWorker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Threading;

namespace BackgroundWorker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackgroundWorkerController : ControllerBase
    {
        private readonly WorkerService _service;

        public BackgroundWorkerController(
            WorkerService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWorkersListAsync(
            CancellationToken cancellationToken)
        {
            var workers = _service.GetWorkers()
                .Select(e => new WorkerModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    TotalCompletedJobs = e.TotalCompletedJobs
                })
                .ToArray();

            return Ok(workers);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetJobsListAsync(
            [FromQuery] JobFilterModel filter,
            CancellationToken cancellationToken)
        {
            return Ok(
                new[]
                {
                    new JobModel(),
                    new JobModel(),
                    new JobModel(),
                });
        }

        [HttpGet("[action]/{jobId}")]
        public async Task<IActionResult> GetJobDetailsAsync(
            [FromRoute] string jobId,
            CancellationToken cancellationToken)
        {
            return Ok(new JobDetailsModel());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateWorkerAsync(
            [FromBody] CreateWorkerModel model,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return BadRequest(new { Error = "Worker name is required!" });
            }

            var worker = _service.CreateWorker(model.Name);

            return Ok(new WorkerModel
            {
                Id = worker.Id,
                Name = worker.Name,
                TotalCompletedJobs = worker.TotalCompletedJobs
            });
        }

        [HttpDelete("[action]/{workerId}")]
        public async Task<IActionResult> DeleteWorkerAsync(
            [FromRoute] string workerId,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(workerId))
            {
                return BadRequest(new { Error = "Worker id is required!" });
            }

            var worker = _service.DeleteWorker(workerId);

            return Ok(new WorkerModel
            {
                Id = worker.Id,
                Name = worker.Name,
                TotalCompletedJobs = worker.TotalCompletedJobs
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateJobAsync(
            [FromBody] CreateJobModel model,
            CancellationToken cancellationToken)
        {
            return Ok(model);
        }

        [HttpDelete("[action]/{jobId}")]
        public async Task<IActionResult> DeleteJobAsync(
          [FromRoute] string jobId,
          CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
