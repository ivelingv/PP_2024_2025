using BackgroundWorker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace BackgroundWorker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackgroundWorkerController : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetWorkersListAsync(
            CancellationToken cancellationToken)
        {
            return Ok(
                new[]
                {
                    new WorkerModel(),
                    new WorkerModel()
                });
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
            return Ok(model);
        }

        [HttpDelete("[action]/{workerId}")]
        public async Task<IActionResult> DeleteWorkerAsync(
            [FromRoute] string workerId,
            CancellationToken cancellationToken)
        {
            return Ok();
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
