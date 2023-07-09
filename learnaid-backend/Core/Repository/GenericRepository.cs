using learnaid_backend.Core.Models.ORM;
using learnaid_backend.Core.Repository.Interfaces;
using learnaid_backend.Data;

namespace learnaid_backend.Core.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private readonly DataContext _dbContext;

        public GenericRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Actualizar(Entity entity)
        {
            _dbContext.Update(entity);
            return Task.CompletedTask;
        }

        public Task Agregar(Entity entity)
        {
            _dbContext.Add(entity);
            return Task.CompletedTask;
        }

        public Task Eliminar(Entity entity)
        {
            _dbContext.Remove(entity);
            return Task.CompletedTask;
        }

        public Task GuardarCambios()
        {
            _dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task GuardarCambiosAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
