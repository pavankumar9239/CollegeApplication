using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Implementations
{
    class StudentRepository : IStudentRepository
    {
        private readonly CollegeDBContext _dbContext;

        public StudentRepository(CollegeDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<int> CreateStudentAsync(Student student)
        {
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteStudentAsync(Student student)
        {
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Student> GetStudentByIdAsync(int id, bool asNoTracking = false)
        {
            if(asNoTracking)
            {
                return await _dbContext.Students.AsNoTracking().Where(Student => Student.Id == id).FirstOrDefaultAsync();
            }
            return await _dbContext.Students.Where(Student => Student.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Student> GetStudentByNameAsync(string name)
        {
            return await _dbContext.Students.Where(x => x.Name.ToLower().Contains(name.ToLower())).FirstOrDefaultAsync();
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<int> UpdateStudentAsync(Student student)
        {
            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }
    }
}
