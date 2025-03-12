using AutoMapper;
using CollegeApplication.Models;
using Repository.Models;

namespace CollegeApplication.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //CreateMap<Student, StudentDto>();
            //CreateMap<StudentDto, Student>();

            //Instead of doing reverse mapping, we can write in one line as below
            CreateMap<Student, StudentDto>().ReverseMap();
        }
    }
}
