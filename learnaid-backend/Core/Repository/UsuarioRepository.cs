using learnaid_backend.Core.DataTransferObjects.Usuario;
using learnaid_backend.Core.Models;
using learnaid_backend.Core.Repository.Interfaces;
using learnaid_backend.Data;
using Microsoft.EntityFrameworkCore;

namespace learnaid_backend.Core.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DataContext _dbContext;
        public UsuarioRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
            public async Task<Usuario> GetUsuarioByEmail(string email)
        {
            return await _dbContext.Usuario
                .SingleOrDefaultAsync(usuario => usuario.Email == email);
        }

        public async Task<Usuario> GetUsuarioById(int id)
        {
            return await _dbContext.Usuario
                .SingleOrDefaultAsync(usuario => usuario.Id == id);
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            return await _dbContext.Usuario
                .ToListAsync();
        }

        public async Task<bool> VerificarCredencialesLogueo(LoguearseDTO credenciales)
        {
            return await _dbContext.Usuario
                .AnyAsync(usuario => usuario.Email == credenciales.Email && usuario.Contraseña == credenciales.Contraseña);
        }
    }
}
