using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp2.Data;
using WpfApp2.Entity;
using WpfApp2.Models;
using WpfApp2.ViewModels.SubjectViewModels;
using WpfApp2.Views.Windows.Subject;

namespace WpfApp2.Services;

public class SubjectDialogService
{
    private IMapper _mapper;

    public SubjectDialogService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public bool Create(SubjectModel subject)
    {
        var vm = new SubjectCreateViewModel(subject);
        var windows = new SubjectCreateWindow
        {
            DataContext = vm
        };

        if (windows.ShowDialog() == false)
        {
            return false;
        }

        subject.Name = vm.Name;

        var context = ContextFactory.CreateContext();
        var sub = _mapper.Map<Subject>(subject);
        var addedSubject = context.Subjects.Add(sub);
        
        subject.Id = addedSubject.Entity.Id;
        foreach (var numGroup in vm.NumGroups)
        {
            context.SubjectGroups.Add(new SubjectGroup
            {
                NumGroup = numGroup,
                Subject = sub
            });
        }

        context.SaveChanges();
        return true;
    }

    public bool Edit(SubjectModel subject)
    {
        var context = ContextFactory.CreateContext();

        var nums = context.SubjectGroups
            .Where(s => s.SubjectId == subject.Id && s.NumGroup != null)
            .Select(s => s.NumGroup)
            .ToList();

        ObservableCollection<int> collection = new();
        foreach (var num in nums)
        {
            collection.Add(num);
        }

        var vm = new SubjectCreateViewModel(subject)
        {
            NumGroups = collection
        };

        var windows = new SubjectCreateWindow
        {
            DataContext = vm
        };

        if (windows.ShowDialog() == false)
        {
            return false;
        }

        var sub = _mapper.Map<Subject>(subject);
        context.Entry(sub).State = EntityState.Modified;

        foreach (var prevNum in nums)
        {
            var prevRow = context.SubjectGroups.First(sg => sg.SubjectId == sub.Id && sg.NumGroup == prevNum);
            context.Entry(prevRow).State = EntityState.Deleted;
        }

        foreach (var newNum in vm.NumGroups)
        {
            context.SubjectGroups.Add(new SubjectGroup
            {
                NumGroup = newNum,
                Subject = sub
            });
        }

        context.SaveChanges();
        return true;
    }
}
