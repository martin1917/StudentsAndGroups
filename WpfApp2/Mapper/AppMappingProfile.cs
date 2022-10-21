using AutoMapper;
using WpfApp2.Models;
using WpfApp2.Entity;

namespace WpfApp2.Mapper;

/// <summary> Класс, отображающий один класс на другой</summary>
public class AppMappingProfile : Profile
{
    /// <summary> Конструтор, в котором происходит конфигурация маппера </summary>
	public AppMappingProfile()
	{
		CreateMap<Group, GroupModel>()
			.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
			.ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
			.ForMember(d => d.DateCreated, opt => opt.MapFrom(s => s.DateCreated))
			.ForMember(d => d.StudentModels, opt => opt.MapFrom(s => s.Students));

        CreateMap<GroupModel, Group>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.DateCreated, opt => opt.MapFrom(s => s.DateCreated))
            .ForMember(d => d.Students, opt => opt.MapFrom(s => s.StudentModels));

        CreateMap<Student, StudentModel>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FirstName))
            .ForMember(d => d.SecondName, opt => opt.MapFrom(s => s.SecondName))
            .ForMember(d => d.Patronymic, opt => opt.MapFrom(s => s.Patronymic))
            .ForMember(d => d.BirthDay, opt => opt.MapFrom(s => s.BirthDay))
            .ForMember(d => d.GroupModel, opt => opt.MapFrom(s => s.Group));
        
        CreateMap<StudentModel, Student>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FirstName))
            .ForMember(d => d.SecondName, opt => opt.MapFrom(s => s.SecondName))
            .ForMember(d => d.Patronymic, opt => opt.MapFrom(s => s.Patronymic))
            .ForMember(d => d.BirthDay, opt => opt.MapFrom(s => s.BirthDay))
            .ForMember(d => d.Group, d => d.Ignore());

        CreateMap<Subject, SubjectModel>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));

        CreateMap<SubjectModel, Subject>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));

        CreateMap<AcademicPerformanceLog, AcademicPerformanceLogModel>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.SubjectId, opt => opt.MapFrom(s => s.SubjectId))
            .ForMember(d => d.GroupId, opt => opt.MapFrom(s => s.GroupId))
            .ForMember(d => d.StudentId, opt => opt.MapFrom(s => s.StudentId))
            .ForMember(d => d.Date, opt => opt.MapFrom(s => s.Date))
            .ForMember(d => d.Mark, opt => opt.MapFrom(s => s.Mark))
            .ForMember(d => d.SubjectModel, opt => opt.MapFrom(s => s.Subject))
            .ForMember(d => d.GroupModel, opt => opt.MapFrom(s => s.Group))
            .ForMember(d => d.StudentModel, opt => opt.MapFrom(s => s.Student));
        
        CreateMap<AcademicPerformanceLogModel, AcademicPerformanceLog>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.SubjectId, opt => opt.MapFrom(s => s.SubjectId))
            .ForMember(d => d.GroupId, opt => opt.MapFrom(s => s.GroupId))
            .ForMember(d => d.StudentId, opt => opt.MapFrom(s => s.StudentId))
            .ForMember(d => d.Date, opt => opt.MapFrom(s => s.Date))
            .ForMember(d => d.Mark, opt => opt.MapFrom(s => s.Mark))
            .ForMember(d => d.Subject, d => d.Ignore())
            .ForMember(d => d.Group, d => d.Ignore())
            .ForMember(d => d.Student, d => d.Ignore());
    }
}
