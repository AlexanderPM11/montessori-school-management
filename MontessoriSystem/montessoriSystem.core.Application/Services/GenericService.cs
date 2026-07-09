using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class GenericService<SaveViewModel, ViewModel, Model> : IGenericService<SaveViewModel, ViewModel, Model>
        where SaveViewModel : class
        where ViewModel : class
        where Model : class
    {

        private readonly IGenericRepository<Model> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Model> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<SaveViewModel> Add(SaveViewModel vm)
        {
            try
            {
                Model entity = _mapper.Map<Model>(vm);

                entity = await _repository.AddAsync(entity);

                SaveViewModel categoryVm = _mapper.Map<SaveViewModel>(entity);
                return categoryVm;
            }
            catch (Exception  ex)
            {

                throw;
            }
            
        }
        public virtual async Task Update(SaveViewModel vm, int id)
        {
            try
            {
                Model entity = _mapper.Map<Model>(vm);

                await _repository.UpdateAsync(entity, id);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public virtual async Task Delete(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                await _repository.DeleteAsync(entity);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }


        public virtual async Task<SaveViewModel> GetByIdSaveViewModel(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            SaveViewModel vm = _mapper.Map<SaveViewModel>(entity);
            return vm;
        }

        public virtual async Task<List<ViewModel>> GetAllViewModel()
        {
            try
            {
                var entityList = await _repository.GetAllAsync();

                return _mapper.Map<List<ViewModel>>(entityList);
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }
        public virtual async Task<List<ViewModel>> GetAllWithIncludeViewModel(List<string> includeTablename, 
            Expression<Func<Model, bool>>? query = null)
        {
            var entityList = await _repository.GetAllWithIncludeAsync(includeTablename, query);

            return _mapper.Map<List<ViewModel>>(entityList);
        } 
        public virtual async Task<List<ViewModel>>  GetBy(Expression<Func<Model, bool>> query)
        {
            var entityList = await _repository.GetBy(query);

            return _mapper.Map<List<ViewModel>>(entityList);
        }



    }

}
