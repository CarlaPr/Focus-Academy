using System.Threading.Tasks;
using FocusAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FocusAcademy.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Obter o JSON da sessão
            string sessaoUsuario = HttpContext.Session.GetString("sessaoUsuarioLogado");

            // Verificar se a sessão é nula ou vazia
            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                // Retornar uma view com um modelo nulo ou uma mensagem apropriada
                return View("Default", (UsuarioModel)null); // Explicitamente especificando o tipo do modelo
            }

            // Desserializar o JSON para o modelo UsuarioModel
            UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);

            // Verificar se o modelo foi desserializado corretamente
            if (usuario == null)
            {
                // Retornar uma view com um modelo nulo ou uma mensagem apropriada
                return View("Default", (UsuarioModel)null); // Explicitamente especificando o tipo do modelo
            }

            // Retornar a view com o modelo
            return View("Default", usuario); // Explicitamente especificando o tipo do modelo
        }
    }
}
