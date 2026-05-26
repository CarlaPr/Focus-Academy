# 🎓 Focus Academy - Projeto Acadêmico

A **Focus Academy** é uma aplicação web desenvolvida para a gestão de treinamentos e cursos institucionais. O sistema oferece uma experiência completa para o estudante, permitindo o gerenciamento de perfil, consulta de grades horárias e o fluxo de matrículas em cursos[cite: 1].

Este projeto demonstra a aplicação de padrões de arquitetura corporativa e gerenciamento de estado no ecossistema .NET.

## 🚀 Principais Funcionalidades

* **Autenticação Segura:** Fluxo de login e logout com proteção de rotas privadas por meio de variáveis de sessão[cite: 1].
* **Painel do Aluno:** Dashboard personalizado que exibe informações contextuais e saudações dinâmicas ao usuário autenticado[cite: 1].
* **Gestão de Matrículas:** Sistema para inscrição em novos cursos e visualização de grade horária atual[cite: 1].
* **Cancelamento de Inscrição:** Mecanismo com dupla confirmação para evitar cancelamentos acidentais[cite: 1].
* **Edição de Perfil:** CRUD completo para atualização de dados cadastrais como endereço, CPF, e-mail e telefone[cite: 1].

## 🛠️ Tecnologias Utilizadas

* **Linguagem:** C#[cite: 1]
* **Framework:** .NET 8.0[cite: 1]
* **Arquitetura:** ASP.NET Core MVC (Model-View-Controller)[cite: 1]
* **Front-end:** Razor Views (HTML5 dinâmico) e CSS[cite: 1]
* **Gerenciamento de Estado:** Sessões (HttpContext Session)[cite: 1]

## 🏗️ Diferenciais do Projeto

* **Repository Pattern:** Uso de repositórios para isolar a lógica de dados, facilitando a manutenção e testes[cite: 1].
* **Tratamento de Erros:** Implementação de blocos try-catch com feedback visual ao usuário através de alertas amigáveis[cite: 1].
* **Validação de Dados:** Uso de Data Annotations para garantir a integridade das informações enviadas nos formulários[cite: 1].

---

### 💡 Como rodar o projeto

1. Tenha o **SDK do .NET 8** instalado.
2. Clone este repositório.
3. Na pasta raiz `FocusAcademy`, abra o terminal e execute:
```bash
   dotnet run
