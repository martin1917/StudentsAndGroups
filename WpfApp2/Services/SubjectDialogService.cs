using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp2.Data;
using WpfApp2.Entity;
using WpfApp2.Models;
using WpfApp2.ViewModels.SubjectDialogVM;
using WpfApp2.Views.Windows.SubjectDialogs;

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
        var windows = new SubjectCreateWindow { DataContext = vm };

        if (windows.ShowDialog() == false)
            return false;

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

        var vm = new SubjectCreateViewModel(subject) { NumGroups = collection };
        var windows = new SubjectCreateWindow { DataContext = vm };

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

    public bool AddSubject(SubjectModel subject, int numGroup)
    {
        var context = ContextFactory.CreateContext();

        var alreadyAdded = context.SubjectGroups
            .Where(sg => sg.NumGroup == numGroup)
            .Select(sg => sg.SubjectId);

        var subjects = context.Subjects.AsEnumerable().Where(s => !alreadyAdded.Contains(s.Id));
        var subjectModels = _mapper.Map<List<SubjectModel>>(subjects);

        var vm = new AddSubjectForGroupViewModel(subjectModels, numGroup);
        var window = new AddSubjectForGroupWindow { DataContext = vm };

        if(window.ShowDialog() == false)
        {
            return false;
        }

        subject.Name = vm.SelectedSubject.Name;
        subject.Id = vm.SelectedSubject.Id;

        var selectedSubject = _mapper.Map<Subject>(vm.SelectedSubject);
        context.SubjectGroups.Add(new SubjectGroup { NumGroup = numGroup, SubjectId = selectedSubject.Id });
        context.SaveChanges();
        return true;
    }
}
