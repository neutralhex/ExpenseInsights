using ExpenseInsights.WebApi.Repositories;
using ExpenseInsights.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseInsights.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IStatementProcessor _statementProcessor;

        public TransactionsController(IStatementProcessor statementProcessor)
        {
            _statementProcessor = statementProcessor;
        }

        [HttpPost]
        [Route("process")]
        public IActionResult ProcessStatement(IFormFile statement)
        {
            if (!_statementProcessor.Upload(statement))
                return BadRequest($"Error uploading file.");

            if (!_statementProcessor.Process(statement.FileName))
                return BadRequest($"Error processing file.");

            return Ok("File processed");
        }
    }
}
