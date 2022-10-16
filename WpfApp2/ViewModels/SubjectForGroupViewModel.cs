using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfApp2.Data;
using WpfApp2.State;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;
using WpfApp2.Services;

namespace WpfApp2.ViewModels;

public class SubjectForGroupViewModel : BaseViewModel
{
    private IMapper _mapper;
    private CommonDialogService _commonDialogService;
    private SubjectDialogService _subjectDialogService;

    public SubjectForGroupViewModel(IMapper mapper,
        CommonDialogService commonDialogService,
        SubjectDialogService subjectDialogService) : base(ViewModelType.SubjectsForGroup)
    {
        _mapper = mapper;
        _commonDialogService = commonDialogService;
        _subjectDialogService = subjectDialogService;
    }

    public ObservableCollection<SubjectModel> SubjectModels { get; } = new();

    public SubjectModel _selectedSubject;
    public SubjectModel SelectedSubject { get => _selectedSubject; set => Set(ref _selectedSubject, value); }

    public List<int> Nums { get; } = Enumerable.Range(1, 11).ToList();

    private int? _selectedNumGroup;
    public int? SelectedNumGroup { get => _selectedNumGroup; set => Set(ref _selectedNumGroup, value); }

    // загрузка предметов
    private ICommand _loadSubjectCommand;
    public ICommand LoadSubjectCommand => _loadSubjectCommand
        ??= new Command(OnLoadSubjectCommandExecuted, CanLoadSubjectCommandExecute);

    private bool CanLoadSubjectCommandExecute(object? param)
        => SelectedNumGroup != null;

    private void OnLoadSubjectCommandExecuted(object? param)
    {
        var context = ContextFactory.CreateContext();

        var subjects = context.SubjectGroups
            .Where(sg => sg.NumGroup == SelectedNumGroup)
            .Include(sg => sg.Subject)
            .Select(sg => sg.Subject);

        var items = _mapper.Map<ObservableCollection<SubjectModel>>(subjects);
        SubjectModels.Clear();
        foreach(var item in items)
        {
            SubjectModels.Add(item);
        }
    }

    // удаление предмета для группы
    private ICommand _deleteSubjectCommand;
    public ICommand DeleteSubjectCommand => _deleteSubjectCommand
        ??= new Command(OnDeleteSubjectCommandExecuted, CanDeleteSubjectCommandExecute);

    private bool CanDeleteSubjectCommandExecute(object? param)
        => SelectedSubject != null;

    private void OnDeleteSubjectCommandExecuted(object? param)
    {
        var context = ContextFactory.CreateContext();

        if (!_commonDialogService.ConfirmInformation($"Вы точно хотите удалить предмет: {SelectedSubject.Name} " +
            $"для {SelectedNumGroup} класса ?", "Удаление предмета"))
        {
            return;
        }
        
        var row = context.SubjectGroups
            .First(sg => sg.NumGroup == SelectedNumGroup && sg.SubjectId == SelectedSubject.Id);

        context.Entry(row).State = EntityState.Deleted;
        context.SaveChanges();

        SubjectModels.Remove(SelectedSubject);
    }

    // добавление предмета для группы
    private ICommand _addSubjectCommand;
    public ICommand AddSubjectCommand => _addSubjectCommand
        ??= new Command(OnAddSubjectCommandExecuted, CanAddSubjectCommandExecute);

    private bool CanAddSubjectCommandExecute(object? param)
        => SelectedNumGroup != null;

    private void OnAddSubjectCommandExecuted(object? param)
    {
        var newSubject = new SubjectModel();
        if(!_subjectDialogService.AddSubject(newSubject, SelectedNumGroup.Value))
        {
            return;
        }

        SubjectModels.Add(newSubject);
    }
}