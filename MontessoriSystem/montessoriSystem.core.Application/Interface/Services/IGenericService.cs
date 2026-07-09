using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Model> 
        where SaveViewModel : class
        where ViewModel : class
        where Model : class
    {
        Task Update(SaveViewModel vm, int id);
        Task<SaveViewModel> Add(SaveViewModel vm);
        Task Delete(int id);
        Task<SaveViewModel> GetByIdSaveViewModel(int id);
        Task<List<ViewModel>> GetAllViewModel();
        Task<List<ViewModel>> GetAllWithIncludeViewModel(List<string> includeTablename, Expression<Func<Model, bool>>? query = null);
        Task<List<ViewModel>> GetBy(Expression<Func<Model, bool>> query);
    }
}
