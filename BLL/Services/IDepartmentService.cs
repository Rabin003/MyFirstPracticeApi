using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Model;
using DLL.Repositories;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface IDepartmentService
    {
        Task<Department> InsertAsync(DepartmentInsertRequestViewModel request);
        Task<List<Department>> GetAllAsync();
        Task<Department> DeleteAsync(string code);
        Task<Department> GetAAsync(string code);
        Task<Department> UpdateAsync(string code, Department department);
        Task<bool> IsCodeExists(string code);
        Task<bool> IsNameExists(string name);
        


    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Department> InsertAsync(DepartmentInsertRequestViewModel request)
        {
            Department aDepartment = new Department();
            aDepartment.Code = request.Code;
            aDepartment.Name = request.Name;

            await _departmentRepository.CreateAsync(aDepartment);
            if (await _departmentRepository.SaveCompletedAsync())
            {
                return aDepartment;

            }
            throw new AplicationValidationException("department insert has some problem");

        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _departmentRepository.GetList();
        }

        public async Task<Department> DeleteAsync(string code)
        {
            var department = await _departmentRepository.FindSingleAsync(x =>x.Code==code);
            if (department == null)
            {
                throw new AplicationValidationException("department not found");
            }

            _departmentRepository.delete(department);

            if (await _departmentRepository.SaveCompletedAsync())
            {
                return department;

            }
            throw new Exception("Some problem for delete data");
            
        }

        public async Task<Department> GetAAsync(string code)
        {
            var department = await _departmentRepository.FindSingleAsync(x =>x.Code==code);
            if (department == null)
            {
                
                throw new AplicationValidationException("department not found");
            }

            return department;
        }

        public async Task<Department> UpdateAsync(string code, Department aDepartment)
        {
            var department = await _departmentRepository.FindSingleAsync(x =>x.Code==code);
            if (department == null)
            {
                throw new AplicationValidationException("department not found");
            }

            if (!string.IsNullOrWhiteSpace(aDepartment.Code))
            {
                var existsAlreadyCode = await _departmentRepository.FindSingleAsync(x =>x.Code==code);
                if (existsAlreadyCode != null)
                {
                    throw new AplicationValidationException("your updated code already present in our system"); 
                    
                }

                department.Code = aDepartment.Code;

            }

            if (!string.IsNullOrWhiteSpace(aDepartment.Name))
            {
                var existsAlreadyCode = await _departmentRepository.FindSingleAsync(x =>x.Name==aDepartment.Name);
                if (existsAlreadyCode != null)
                {
                    throw new AplicationValidationException("your updated code already present in our system");

                }

                department.Name = aDepartment.Name;
            }

            _departmentRepository.update(department);
            
            if (await _departmentRepository.SaveCompletedAsync())
            {
                return department;

            }
            throw new AplicationValidationException("in update have some problem");
        }

        public async Task<bool> IsCodeExists(string code)
        {
            var department = await _departmentRepository.FindSingleAsync(x =>x.Code==code);
            if (department==null)
            {
                return true;

            }

            return false;
        }

        public async Task<bool> IsNameExists(string name)
        {
            var department = await _departmentRepository.FindSingleAsync(x =>x.Name==name);
            if (department==null)
            {
                return true;

            }

            return false;
        }
    }
    
}