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

            //If there are different property name from entity class to dto class
            //For ForMember method, intial property will be from destination class and mapfrom will be from source class.
            //CreateMap<Student, StudentDto>().ForMember(n => n.StudentName, opt => opt.MapFrom(n => n.Name)).ReverseMap();


            //After reversemap, source and destination will be swapped so should adjust property names accordingly.
            //CreateMap<Student, StudentDto>().ReverseMap().ForMember(n => n.Name, opt => opt.MapFrom(n => n.StudentName));

            //To ignore mapping for a particular property, we should use ignore method. This will be ignored when we want to ignore when mapping from StudentDto to Student since used after reversemap.
            //CreateMap<Student, StudentDto>().ReverseMap().ForMember(n => n.Name, opt => opt.Ignore());

            //This will be ignored when we want to ignore when mapping from Student to StudentDto since used after reversemap.
            //CreateMap<Student, StudentDto>().ForMember(n => n.Name, opt => opt.Ignore()).ReverseMap();

            //If we want to return some thing by putting condiftions for enti class, we can use transform.
            //CreateMap<Student, StudentDto>().AddTransform<string>(n => string.IsNullOrEmpty(n) ? "No data found" : n).ReverseMap();

            //If we want to apply for particular property.
            //CreateMap<Student, StudentDto>().ForMember(n => n.Address, opt => opt.MapFrom(n => string.IsNullOrEmpty(n.Address) ? "No address found" : n.Address)).ReverseMap();

            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}
