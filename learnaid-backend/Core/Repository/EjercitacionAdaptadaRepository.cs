using learnaid_backend.Core.Models;
using learnaid_backend.Core.Repository.Interfaces;
using learnaid_backend.Data;
using Microsoft.EntityFrameworkCore;

namespace learnaid_backend.Core.Repository
{
    public class EjercitacionAdaptadaRepository : IEjercitacionAdaptadaRepository
    {
        private readonly DataContext _dbContext;

        public EjercitacionAdaptadaRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<EjercitacionAdaptada> GetAdaptadoById(int id)
        {
            return await _dbContext.EjercitacionAdaptada
                .Include(e => e.Ejercicios)
                .Include(e => e.EjercicioOriginal)
                    .ThenInclude(e => e.Ejercicios)
                        .ThenInclude(a => a.Adaptaciones)
                .SingleOrDefaultAsync(ejercicio => ejercicio.Id == id);
        }

        public async Task<List<EjercitacionAdaptada>> GetAdaptados()
        {
            return await _dbContext.EjercitacionAdaptada
                .Include(e => e.Ejercicios)
                .Include(e => e.EjercicioOriginal)
                    .ThenInclude(e => e.Ejercicios)
                        .ThenInclude(a => a.Adaptaciones)
                .ToListAsync();
        }

        public Task<List<EjercitacionAdaptada>> GetAdaptadosByUser(int userid)
        {
            throw new NotImplementedException();
        }

        /*public async Task<List<EjercitacionAdaptada>> GetAdaptadosByUser(int userid)
        {
            var result = new List<EjercitacionAdaptada>();
            var ejercicios = await GetAdaptados();
            ejercicios.ForEach(ejercicio =>
            {
                if (ejercicio.Usuario.Id == userid)
                    result.Add(ejercicio);
            });
            return result;
        }*/
    }
}
