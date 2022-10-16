using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfApp2.Data;
using WpfApp2.State;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.Models;
using WpfApp2.Entity;
using WpfApp2.Services;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels;

public class AllSubjectsViewModel : BaseViewModel
{
	private IMapper _mapper;
	private SubjectDialogService _subjectDialogService;
    private CommonDialogService _commonDialogService;

    public AllSubjectsViewModel(IMapper mapper,
        SubjectDialogService subjectDialogService,
        CommonDialogService commonDialogService) : base(ViewModelType.AllSubjects)
	{
		_mapper = mapper;
		_subjectDialogService = subjectDialogService;
        _commonDialogService = commonDialogService;
        LoadData();
	}

	private void LoadData()
	{
		var context = ContextFactory.CreateContext();
		var items = _mapper.Map<ObservableCollection<SubjectModel>>(context.Subjects.ToList());
		SubjectModels.Clear();
		foreach (var item in items)
		{
			SubjectModels.Add(item);
		}
	}

	public ObservableCollection<SubjectModel> SubjectModels { get; } = new();


	public SubjectModel _selectedSubject;
	public SubjectModel SelectedSubject { get => _selectedSubject; set => Set(ref _selectedSubject, value); }

	// Создание предмета
	private ICommand _createSubjectCommand;
	public ICommand CreateSubjectCommand => _createSubjectCommand
		??= new Command(OnCreateSubjectCommandExecuted);

    private void OnCreateSubjectCommandExecuted(object? param)
	{
		var newSubject = new SubjectModel();

		if (!_subjectDialogService.Create(newSubject))
		{
			return;
		}

		SubjectModels.Add(newSubject);
    }

    // Редактирование предмета
    private ICommand _editSubjectCommand;
    public ICommand EditSubjectCommand => _editSubjectCommand
        ??= new Command(OnEditSubjectCommandExecuted, CanEditSubjectCommandExecute);

	private bool CanEditSubjectCommandExecute(object? param)
		=> SelectedSubject != null;

	private void OnEditSubjectCommandExecuted(object? param)
    {
		if (!_subjectDialogService.Edit(SelectedSubject))
        {
            return;
        }
    }

    // удаление предмета
    private ICommand _deleteSubjectCommand;
    public ICommand DeleteSubjectCommand => _deleteSubjectCommand
        ??= new Command(OnDeleteSubjectCommandExecuted, CanDeleteSubjectCommandExecute);

    private bool CanDeleteSubjectCommandExecute(object? param)
        => SelectedSubject != null;

    private void OnDeleteSubjectCommandExecuted(object? param)
    {
        if (!_commonDialogService.ConfirmInformation($"Вы точно хотите удалить предмет ?\n" +
            $"Предмет: {SelectedSubject.Name}\n", "Удаление группы"))
        {
            return;
        }

        var context = ContextFactory.CreateContext();
        var subject = _mapper.Map<Subject>(SelectedSubject);

        var SubjectGroups = context.SubjectGroups.Where(sg => sg.SubjectId == subject.Id);
        foreach (var subjectGroup in SubjectGroups)
        {
            context.Entry(subjectGroup).State = EntityState.Deleted;
        }

        context.Entry(subject).State = EntityState.Deleted;
        context.SaveChanges();

        SubjectModels.Remove(SelectedSubject);
    }
}
