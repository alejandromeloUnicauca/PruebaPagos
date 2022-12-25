using Microsoft.EntityFrameworkCore;
using Prueba.Models;
using Prueba.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba.Facade
{
    public class UnitOfWorkRepositories : IDisposable
    {
        private ZonapagosContext dbContext;
        private GenericRepository<Comercio> comercioRepository;
        private GenericRepository<Usuario> usuarioRepository;
        private GenericRepository<Trans> transRepository;
        private GenericRepository<TransEstado> transEstadoRepository;
        private GenericRepository<TransMedioPago> transMedioPagoRepository;

        public UnitOfWorkRepositories(ZonapagosContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public GenericRepository<Comercio> ComercioRepository
        {
            get
            {
                if (this.comercioRepository == null)
                    this.comercioRepository = new GenericRepository<Comercio>(dbContext);
                return this.comercioRepository;
            }
        }

        public GenericRepository<Usuario> UsuarioRepository
        {
            get
            {
                if (this.usuarioRepository == null)
                    this.usuarioRepository = new GenericRepository<Usuario>(dbContext);
                return this.usuarioRepository;
            }
        }

        public GenericRepository<Trans> TransRepository
        {
            get
            {
                if (this.transRepository == null)
                    this.transRepository = new GenericRepository<Trans>(dbContext);
                return this.transRepository;
            }
        }

        public GenericRepository<TransMedioPago> TransMedioPagoRepository
        {
            get
            {
                if (this.transMedioPagoRepository == null)
                    this.transMedioPagoRepository = new GenericRepository<TransMedioPago>(dbContext);
                return this.transMedioPagoRepository;
            }
        }

        public GenericRepository<TransEstado> TransEstadoRepository
        {
            get
            {
                if (this.transEstadoRepository == null)
                    this.transEstadoRepository = new GenericRepository<TransEstado>(dbContext);
                return this.transEstadoRepository;
            }
        }
        public async Task Save()
        {
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e) { throw e; }
            catch (Exception ex) { throw ex.InnerException; }
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
