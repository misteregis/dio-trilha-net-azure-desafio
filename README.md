# DIO - Trilha .NET - Nuvem com Microsoft Azure
www.dio.me

## Desafio de projeto
Para este desafio, você precisará usar seus conhecimentos adquiridos no módulo de Nuvem com Microsoft Azure, da trilha .NET da DIO.

## Contexto
Você precisa construir um sistema de RH, onde para essa versão inicial do sistema o usuário poderá cadastrar os funcionários de uma empresa.

Essa cadastro precisa precisa ter um CRUD, ou seja, deverá permitir obter os registros, criar, salvar e deletar esses registros. A sua aplicação também precisa armazenar logs de toda e qualquer alteração que venha a ocorrer com um funcionário.

## Premissas
A sua aplicação deverá ser do tipo Web API, Azure Functions ou MVC, fique a vontade para implementar a solução que achar mais adequado.

A sua aplicação deverá ser implantada no Microsoft Azure, utilizando o App Service para a API, SQL Database para o banco relacional e Azure Table para armazenar os logs.

A sua aplicação deverá armazenar os logs de todas as alterações que venha a acontecer com o funcionário. Os logs deverão serem armazenados em uma Azure Table.

A sua classe principal, a classe Funcionario e a FuncionarioLog, deve ser a seguinte:

![Diagrama da classe Funcionario](https://github.com/misteregis/dio-trilha-net-azure-desafio/assets/9176161/d7262b41-2349-4c4c-b8e5-11346dd8c5ca)


A classe FuncionarioLog é filha da classe Funcionario, pois o log terá as mesmas informações da Funcionario.

Não se esqueça de gerar a sua migration para atualização no banco de dados.

## Métodos esperados
É esperado que você crie o seus métodos conforme a seguir:

**Endpoints**

| Verbo   | Endpoint                | Parâmetro | Body               |
|---------|-------------------------|-----------|--------------------|
| GET     | /api/employee/{id}      | id        | N/A                |
| PUT     | /api/employee/{id}      | id        | Schema Funcionario |
| DELETE  | /api/employee/{id}      | id        | N/A                |
| POST    | /api/employee           | N/A       | Schema Funcionario |
| **GET** | **/api/employee**       | **N/A**   | **N/A**            |


**Swagger**

![Métodos Swagger](https://github.com/misteregis/dio-trilha-net-azure-desafio/assets/9176161/185b68da-4e68-4d2d-9690-d361b8d46ba5)

Esse é o schema (model) de Funcionario, utilizado para passar para os métodos que exigirem:

```json
{
  "nome": "Nome funcionario",
  "endereco": "Rua 1234",
  "ramal": "1234",
  "emailProfissional": "email@email.com",
  "departamento": "TI",
  "salario": 1000,
  "dataAdmissao": "2022-06-23T02:58:36.345Z"
}
```

## Ambiente
Este é um diagrama do ambiente que deverá ser montado no Microsoft Azure, utilizando o App Service para a API, SQL Database para o banco relacional e Azure Table para armazenar os logs.

![Diagrama da classe Funcionario](https://github.com/misteregis/dio-trilha-net-azure-desafio/assets/9176161/3c41718a-07f8-48f9-9dad-18f3da09b313)


## Solução
O código está pela metade, e você deverá dar continuidade obedecendo as regras descritas acima, para que no final, tenhamos um programa funcional. Procure pela palavra comentada "TODO" no código, em seguida, implemente conforme as regras acima, incluindo a sua publicação na nuvem.

## Solucionado!
O código está completo e funcional.

## O que foi alterado

- _Alterado o framework net6.0 para net7.0 e atualizado alguns pacotes;_
- _Alterado configuração do Swagger no arquivo [Program.cs](Program.cs);_
- _Criado algumas regras no modelo [Funcionario.cs](Models/Funcionario.cs);_
- _Adicionado alguns atributos no controller [FuncionarioController.cs](Controllers/FuncionarioController.cs);_
- _Criado um novo [Endpoint](Controllers/FuncionarioController.cs?plain=1#L77) (GET /api/employee HTTP/1.1) onde retornará uma lista com todos os funcionários;_
