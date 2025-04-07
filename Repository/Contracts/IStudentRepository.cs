using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IStudentRepository : ICollegeRepository<Student>
    {
        Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus);
    }
}
