using iText.Layout;
using learnaid_backend.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace learnaid_backend.Core.Integrations.iText.Interfaces
{
    public interface IiText
    {
        public Stream GenerarPdF(EjercicioAdaptado ejercicio);
    }
}
