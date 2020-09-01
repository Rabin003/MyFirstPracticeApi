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
            return await _departmentRepository.InsertAsync(aDepartment);

        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }

        public async Task<Department> DeleteAsync(string code)
        {
            var department = await _departmentRepository.GetAAsync(code);
            if (department == null)
            {
                throw new AplicationValidationException("department not found");
            }

            if (await _departmentRepository.DeleteAsync(department))
            {
                return department;

            }
            throw new Exception("Some problem for delete data");
            
        }

        public async Task<Department> GetAAsync(string code)
        {
            var department = await _departmentRepository.GetAAsync(code);
            if (department == null)
            {
                
                throw new AplicationValidationException("department not found");
            }

            return department;
        }

        public async Task<Department> UpdateAsync(string code, Department aDepartment)
        {
            var department = await _departmentRepository.GetAAsync(code);
            if (department == null)
            {
                throw new AplicationValidationException("department not found");
            }

            if (!string.IsNullOrWhiteSpace(aDepartment.Code))
            {
                var existsAlreadyCode = await _departmentRepository.FindByCode(aDepartment.Code);
                if (existsAlreadyCode != null)
                {
                    throw new AplicationValidationException("your updated code already present in our system"); 
                    
                }

                department.Code = aDepartment.Code;

            }

            if (!string.IsNullOrWhiteSpace(aDepartment.Name))
            {
                var existsAlreadyCode = await _departmentRepository.FindByName(aDepartment.Name);
                if (existsAlreadyCode != null)
                {
                    throw new AplicationValidationException("your updated code already present in our system");

                }

                department.Name = aDepartment.Name;
            }

            if (await _departmentRepository.UpdateAsync(department))
            {
                return department;

            }
            throw new AplicationValidationException("in update have some problem");
        }

        public async Task<bool> IsCodeExists(string code)
        {
            var department = await _departmentRepository.FindByCode(code);
            if (department==null)
            {
                return true;

            }

            return false;
        }

        public async Task<bool> IsNameExists(string name)
        {
            var department = await _departmentRepository.FindByName(name);
            if (department==null)
            {
                return true;

            }

            return false;
        }
    }
}