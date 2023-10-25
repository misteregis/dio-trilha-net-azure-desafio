using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrilhaNetAzureDesafio.Models;

public class Funcionario
{
    public Funcionario() { }

    public Funcionario(int id, string nome, string endereco, string ramal, string emailProfissional, string departamento, decimal salario, DateTime dataAdmissao)
    {
        Id = id;
        Nome = nome;
        Endereco = endereco;
        Ramal = ramal;
        EmailProfissional = emailProfissional;
        Departamento = departamento;
        Salario = salario;
        DataAdmissao = dataAdmissao;
    }

    [Key]
    public int Id { get; internal set; }

    [Required, DisplayName("nome")]
    [MaxLength(30, ErrorMessage = "O campo nome deve ter um comprimento máximo de '30' caracteres.")]
    public string Nome { get; set; }

    public string Endereco { get; set; }

    public string Ramal { get; set; }

    [EmailAddress(ErrorMessage = "O campo emailProfissional não é um endereço de e-mail válido.")]
    public string EmailProfissional { get; set; }

    public string Departamento { get; set; }

    public decimal Salario { get; set; }

    public DateTimeOffset? DataAdmissao { get; set; }
}
