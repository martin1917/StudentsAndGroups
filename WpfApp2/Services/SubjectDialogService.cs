using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WpfApp2.Data;
using WpfApp2.Models;
using WpfApp2.ViewModels.SubjectDialogVM;
using WpfApp2.Views.Windows.SubjectDialogs;

namespace WpfApp2.Services;

/// <summary> Диалоговый сервис для ПРЕДМЕТОВ </summary>
public class SubjectDialogService
{
    private IMapper _mapper;

    public SubjectDialogService(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Отображение диалогового окна - для редактирование
    /// </summary>
    /// <param name="subject"></param>
    /// <returns>true - если редактирование удачно; false - иначе</returns>
    public SubjectDTO Edit(SubjectModel subject)
    {
        var vm = new SubjectCreateViewModel(subject);
        var windows = new SubjectCreateWindow { DataContext = vm };

        if (windows.ShowDialog() == false)
        {
            return null;
        }

        return new SubjectDTO(vm.Name, vm.NumGroups);
    }

    /// <summary>
    /// Отображение диалогового окна - для добавления предмета для указанного класса
    /// </summary>
    /// <param name="numGroup">номер класса</param>
    /// <returns>true - если добавление удачно; false - иначе</returns>
    public SubjectModel? AddSubject(int numGroup)
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
            return null;
        }

        return vm.SelectedSubject;
    }
}
