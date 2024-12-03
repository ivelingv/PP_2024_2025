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
        private readonly WorkerService _workerService;
        private readonly JobSevice _jobSevice;

        public BackgroundWorkerController(
            WorkerService workerService,
            JobSevice jobSevice)
        {
            _workerService = workerService;
            _jobSevice = jobSevice;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWorkersListAsync(
            CancellationToken cancellationToken)
        {
            var workers = _workerService.GetWorkers()
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
            var jobs = _jobSevice.GetJobs(
                    jobName: filter.JobName,
                    priority: (Priority?)filter.Priority,
                    status: (Status?)filter.Status)
                .Select(e => e.ToModel())
                .ToArray();

            return Ok(jobs);
        }

        [HttpGet("[action]/{jobId}")]
        public async Task<IActionResult> GetJobDetailsAsync(
            [FromRoute] string jobId,
            CancellationToken cancellationToken)
        {
            var job = _jobSevice.GetJob(jobId)
                .ToDetailModel();
            if (job == null) 
            { 
                return NotFound(); 
            }

            return Ok(job);
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

            var worker = _workerService.CreateWorker(model.Name);

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

            var worker = _workerService.DeleteWorker(workerId);

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
            var job = _jobSevice.CreateJob(
                model.Name!,
                model.Description!,
                model.TimeNeededInSeconds,
                (Priority)model.Priority);

            return Ok(job.ToDetailModel());
        }

        [HttpDelete("[action]/{jobId}")]
        public async Task<IActionResult> DeleteJobAsync(
          [FromRoute] string jobId,
          CancellationToken cancellationToken)
        {
            try
            {
                var job = _jobSevice.DeleteJob(jobId);
                return Ok(job.ToDetailModel());
            }
            catch(Exception ex)
            {
                return NotFound($"Job not found [{ex.Message}]");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PauseWorkersAsync()
        {
            if (!GlobalLock.ResetEvent.WaitOne(0))
            {
                GlobalLock.ResetEvent.Set();
            }
            else
            {
                GlobalLock.ResetEvent.Reset();
            }
            return Ok();
        }
    }
}
