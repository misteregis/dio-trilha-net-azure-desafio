using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using TrilhaNetAzureDesafio.Context;
using TrilhaNetAzureDesafio.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;

namespace TrilhaNetAzureDesafio.Controllers;

[ApiController]
[Tags("Funcionário")]
[Route("api/employee")]
[Produces("application/json")]
public class FuncionarioController : ControllerBase
{
    private readonly RHContext _context;
    private readonly string _connectionString;
    private readonly string _tableName;

    public FuncionarioController(RHContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetValue<string>("ConnectionStrings:SAConnectionString");
        _tableName = configuration.GetValue<string>("ConnectionStrings:AzureTableName");
    }

    private TableClient GetTableClient()
    {
        var serviceClient = new TableServiceClient(_connectionString);
        var tableClient = serviceClient.GetTableClient(_tableName);

        tableClient.CreateIfNotExists();
        return tableClient;
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation("Obter funcionário")]
    [ProducesResponseType(typeof(Funcionario), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public IActionResult ObterPorId(int id)
    {
        var funcionario = _context.Funcionarios.Find(id);

        if (funcionario == null)
            return NotFound();

        return Ok(funcionario);
    }

    [HttpPost]
    [SwaggerOperation("Criar funcionário")]
    [ProducesResponseType(typeof(Funcionario), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
    public IActionResult Criar(Funcionario funcionario)
    {
        var email = funcionario.EmailProfissional;

        if (EmailExiste(email))
            return Conflict($"O endereço de e-mail {email} já está em uso.");

        _context.Funcionarios.Add(funcionario);
        _context.SaveChanges();

        TableClient tableClient = GetTableClient();
        FuncionarioLog funcionarioLog = new(funcionario, TipoAcao.Inclusao, funcionario.Departamento, Guid.NewGuid().ToString());

        tableClient.UpsertEntity(funcionarioLog);

        return CreatedAtAction(nameof(ObterPorId), new { id = funcionario.Id }, funcionario);
    }

    [HttpGet]
    [SwaggerOperation("Obter todos os funcionários")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(IEnumerable<Funcionario>), StatusCodes.Status200OK)]
    public IActionResult ObterTodosFuncionarios()
    {
        if (!_context.Funcionarios.Any())
            return NoContent();

        return Ok(_context.Funcionarios.ToList());
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation("Atualizar funcionário")]
    [ProducesResponseType(typeof(Funcionario), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public IActionResult Atualizar(int id, [FromBody] Funcionario funcionario)
    {
        if (!FuncionarioExiste(id))
            return BadRequest();

        var email = funcionario.EmailProfissional;

        if (EmailExiste(email))
            return Conflict($"O endereço de e-mail '{email}' já está em uso.");

        funcionario.Id = id;

        _context.Entry(funcionario).State = EntityState.Modified;
        _context.SaveChanges();

        TableClient tableClient = GetTableClient();
        FuncionarioLog funcionarioLog = new(funcionario, TipoAcao.Atualizacao, funcionario.Departamento, Guid.NewGuid().ToString());

        tableClient.UpsertEntity(funcionarioLog);

        return Ok(funcionario);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Excluir funcionário")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public IActionResult Deletar(int id)
    {
        var funcionarioBanco = _context.Funcionarios.Find(id);

        if (funcionarioBanco == null)
            return BadRequest();

        _context.Remove(funcionarioBanco);
        _context.SaveChanges();

        TableClient tableClient = GetTableClient();
        FuncionarioLog funcionarioLog = new(funcionarioBanco, TipoAcao.Remocao, funcionarioBanco.Departamento, Guid.NewGuid().ToString());

        tableClient.DeleteEntity(funcionarioLog.PartitionKey, funcionarioLog.RowKey);

        return NoContent();
    }

    private bool FuncionarioExiste(int id)
    {
        return _context.Funcionarios.Any(funcionario => funcionario.Id == id);
    }

    private bool EmailExiste(string email)
    {
        return _context.Funcionarios.Any(funcionario =>
            funcionario.EmailProfissional.ToLower() == email.ToLower()
        );
    }
}
