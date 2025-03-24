using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Implementations
{
    class StudentRepository : CollegeRepository<Student>, IStudentRepository
    {
        private readonly CollegeDBContext _dbContext;

        public StudentRepository(CollegeDBContext dBContext) : base(dBContext)
        {
            _dbContext = dBContext;
        }

        public Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus)
        {
            //write to code to return staudent based on feestatus
            return null;
        }
    }
}
