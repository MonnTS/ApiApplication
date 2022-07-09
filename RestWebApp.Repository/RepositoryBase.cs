using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RestWebApp.Contracts;
using RestWebApp.Entities;

namespace RestWebApp.Repository;

public abstract class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : class
{
    private RepositoryContext RepositoryContext { get; }
    private bool _isDisposed;

    protected RepositoryBase(RepositoryContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
    }

    public IQueryable<T> GetAll()
    {
        return RepositoryContext.Set<T>().AsNoTracking();
    }

    public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
    {
        return RepositoryContext.Set<T>().Where(expression).AsNoTracking();
    }

    public void Create(T entity)
    { 
        RepositoryContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        RepositoryContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        RepositoryContext.Set<T>().Remove(entity);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if(!_isDisposed && disposing)
                RepositoryContext.Dispose();
        
        _isDisposed = true;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}