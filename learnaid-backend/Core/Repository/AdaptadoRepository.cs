using learnaid_backend.Core.Models;
using learnaid_backend.Core.Repository.Interfaces;
using learnaid_backend.Data;
using Microsoft.EntityFrameworkCore;

namespace learnaid_backend.Core.Repository
{
    public class AdaptadoRepository : IAdaptadoRepository
    {
        private readonly DataContext _dbContext;

        public AdaptadoRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<EjercicioAdaptado> GetAdaptadoById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<EjercicioAdaptado>> GetAdaptados()
        {
            throw new NotImplementedException();
        }

        public Task<List<EjercicioAdaptado>> GetAdaptadosByUser(int userid)
        {
            throw new NotImplementedException();
        }

        /*
            public async Task<EjercicioAdaptado> GetAdaptadoById(int id)
            {
                return await _dbContext.EjercicioAdaptado
                    .Include(u => u.Usuario)
                    .SingleOrDefaultAsync(ejercicio => ejercicio.Id == id);
            }

            public async Task<List<EjercicioAdaptado>> GetAdaptados()
            {
                return await _dbContext.EjercicioAdaptado
                    .Include(u => u.Usuario)
                    .ToListAsync();
            }

            public async Task<List<EjercicioAdaptado>> GetAdaptadosByUser(int userid)
            {
                var result = new List<EjercicioAdaptado>();
                var ejercicios = await GetAdaptados();
                ejercicios.ForEach(ejercicio =>
                    {if (ejercicio.Usuario.Id == userid)
                            result.Add(ejercicio);
                    });
                return result;
            }*/
    }
}
