# 🎓 Focus Academy - Projeto Acadêmico

A **Focus Academy** é uma aplicação web desenvolvida para a gestão de treinamentos e cursos institucionais. O sistema oferece uma experiência completa para o estudante, permitindo o gerenciamento de perfil, consulta de grades horárias e o fluxo de matrículas em cursos.

Este projeto demonstra a aplicação de padrões de arquitetura corporativa e gerenciamento de estado no ecossistema .NET.

## 🚀 Principais Funcionalidades

* **Autenticação Segura:** Fluxo de login e logout com proteção de rotas privadas por meio de variáveis de sessão.
* **Painel do Aluno:** Dashboard personalizado que exibe informações contextuais e saudações dinâmicas ao usuário autenticado.
* **Gestão de Matrículas:** Sistema para inscrição em novos cursos e visualização de grade horária atual.
* **Cancelamento de Inscrição:** Mecanismo com dupla confirmação para evitar cancelamentos acidentais.
* **Edição de Perfil:** CRUD completo para atualização de dados cadastrais como endereço, CPF, e-mail e telefone.

## 🛠️ Tecnologias Utilizadas

* **Linguagem:** C#
* **Framework:** .NET 8.0
* **Arquitetura:** ASP.NET Core MVC (Model-View-Controller)
* **Front-end:** Razor Views (HTML5 dinâmico) e CSS
* **Gerenciamento de Estado:** Sessões (HttpContext Session)

## 🏗️ Diferenciais do Projeto

* **Repository Pattern:** Uso de repositórios para isolar a lógica de dados, facilitando a manutenção e testes.
* **Tratamento de Erros:** Implementação de blocos try-catch com feedback visual ao usuário através de alertas amigáveis.
* **Validação de Dados:** Uso de Data Annotations para garantir a integridade das informações enviadas nos formulários.

---

### 💡 Como rodar o projeto

1. Tenha o **SDK do .NET 8** instalado.
2. Clone este repositório.
3. Na pasta raiz `FocusAcademy`, abra o terminal e execute:
```bash
   dotnet run
