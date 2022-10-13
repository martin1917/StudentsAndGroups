using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp2.Entity;

namespace WpfApp2.Data;

public class DbInitializer
{
	private Context _ctx;

	public DbInitializer(Context ctx) => _ctx = ctx;

    public void Initialize()
    {
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        var rnd = new Random();
        var initDate = new DateOnly(2015, 9, 1);

        // Добавление групп
        var groups = Enumerable.Range(1, 10).Select(i => new Group
        {
            Name = $"Group: {i}",
            DateCreated = initDate.AddYears(rnd.Next(0, 8))
        }).ToList();
        _ctx.Groups.AddRange(groups);

        // Добавление студентов
        var students = new List<Student>();
        for (int i = 0; i < groups.Count(); i++)
        {
            var group = groups[i];
            for (int j = 0; j < 10; j++)
            {
                students.Add(new Student
                {
                    FirstName = $"First Name: {10 * i + j}",
                    SecondName = $"Second Name {10 * i + j}",
                    Patronymic = $"Patronymic: {10 * i + j}",
                    BirthDay = DateOnly.FromDateTime(DateTime.Now.AddYears(-1 * rnd.Next(10, 20))),
                    Group = group
                });
            }
        }
        _ctx.Students.AddRange(students);

        // Добавление предметов
        string[] subjectsName = { "math", "algebra", "himia", "language", "phisic", "drawing", "sports", "astronomics" };
        var subjects = new List<Subject>();
        foreach (var subject in subjectsName)
        {
            subjects.Add(new Subject
            {
                Name = subject
            });
        }
        _ctx.Subjects.AddRange(subjects);

        // указание какие группы изучают какой предмет
        var subjectGroups = new List<SubjectGroup>();
        foreach (var subject in subjects)
        {
            var numGroups = Enumerable.Range(1, rnd.Next(1, 6))
                .Select(_ => rnd.Next(1, 9))
                .Distinct();

            foreach (var numGroup in numGroups)
            {
                subjectGroups.Add(new SubjectGroup
                {
                    Subject = subject,
                    NumGroup = numGroup
                });
            }
        }
        _ctx.SubjectGroups.AddRange(subjectGroups);

        // добавление отметок
        var startDate = new DateTime(2022, 10, 1);
        var academicPerformanceLogs = new List<AcademicPerformanceLog>();
        for (int i = 0; i < 500; i++)
        {
            var group = groups[rnd.Next(0, groups.Count())];

            var studentsInGroup = students.Where(s => s.Group.Name.Equals(group.Name)).ToList();
            var student = studentsInGroup[rnd.Next(0, studentsInGroup.Count())];

            var date = DateOnly.FromDateTime(startDate.AddDays(rnd.Next(0, 31)));

            int? mark = rnd.Next(1, 6);
            mark = mark == 1 ? null : mark;

            int deltaYear = DateTime.Now.Year - group.DateCreated.Year + 1;
            int deltaMonth = DateTime.Now.Month - group.DateCreated.Month;
            int numGroup = deltaMonth < 0 ? deltaYear - 1 : deltaYear;
            var subjectsInGroup = subjectGroups.Where(sg => sg.NumGroup == numGroup).ToList();
            var subject = subjectsInGroup[rnd.Next(0, subjectsInGroup.Count())].Subject;

            academicPerformanceLogs.Add(new AcademicPerformanceLog
            {
                Subject = subject,
                Group = group,
                Student = student,
                Date = date,
                Mark = mark
            });
        }
        _ctx.AcademicPerformanceLogs.AddRange(academicPerformanceLogs);
        
        _ctx.SaveChanges();
    }
}
