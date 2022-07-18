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
        private readonly IStatementService _statementService;

        public TransactionsController(IStatementService statementService)
        {
            _statementService = statementService;
        }

        [HttpPost]
        [Route("process")]
        public IActionResult ProcessStatement(IFormFile statement)
        {
            if (!_statementService.Upload(statement))
                return BadRequest($"Error uploading file.");

            if (!_statementService.Process(statement.FileName))
                return BadRequest($"Error processing file.");

            return Ok("File processed");
        }

        [HttpGet]
        public IActionResult GetAllTransactions()
        {
            return Ok(_statementService.GetAllTransactions());
        }
    }
}
