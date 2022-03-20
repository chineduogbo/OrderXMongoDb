using OrderXMongoWebApi.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderXMongoWebApi.Services.Interface
{
   public interface iCRUDService<T> where T : class
    {
        Task<bool> Create(T model,string ClassName);
       // Task<SuccessDTO> Edit(T model, string ClassName);
        Task<T> GetById(string Id, string ClassName);
        Task<IEnumerable<T>> GetAll(string ClassName);
        Task<SuccessDTO> Delete(string Id, string ClassName);
        
    }
}
