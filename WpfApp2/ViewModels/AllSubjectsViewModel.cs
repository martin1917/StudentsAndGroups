using AutoMapper;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfApp2.Data;
using WpfApp2.State;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;
using WpfApp2.Managers;

namespace WpfApp2.ViewModels;

/// <summary> VM со всеми предметами </summary>
public class AllSubjectsViewModel : BaseViewModel
{
	private IMapper _mapper;
    private SubjectManager _subjectManager;
    private SubjectModel _selectedSubject;
    private ICommand _createSubjectCommand;
    private ICommand _editSubjectCommand;
    private ICommand _deleteSubjectCommand;

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="mapper">Маппер</param>
	/// <param name="subjectManager">Менеджер для работы с предметами</param>
    public AllSubjectsViewModel(IMapper mapper,
        SubjectManager subjectManager) : base(ViewModelType.AllSubjects)
	{
		_mapper = mapper;
        _subjectManager = subjectManager;
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

	/// <summary> предметы </summary>
	public ObservableCollection<SubjectModel> SubjectModels { get; } = new();

	/// <summary> Выбранный предмет </summary>
	public SubjectModel SelectedSubject { get => _selectedSubject; set => Set(ref _selectedSubject, value); }

    /// <summary> Создание предмета </summary>
    public ICommand CreateSubjectCommand => 
		_createSubjectCommand ??= new Command(OnCreateSubjectCommandExecuted);

    private void OnCreateSubjectCommandExecuted(object? param)
	{
        var newSubject = _subjectManager.Create();
		if (newSubject == null) return;
		SubjectModels.Add(newSubject);
    }

    /// <summary> Редактирование предмета </summary>
    public ICommand EditSubjectCommand => 
		_editSubjectCommand ??= new Command(OnEditSubjectCommandExecuted, CanEditSubjectCommandExecute);

	private bool CanEditSubjectCommandExecute(object? param)
		=> SelectedSubject != null;

	private void OnEditSubjectCommandExecuted(object? param)
    {
        _subjectManager.Edit(SelectedSubject);
    }

    /// <summary> удаление предмета </summary>
    public ICommand DeleteSubjectCommand => 
		_deleteSubjectCommand ??= new Command(OnDeleteSubjectCommandExecuted, CanDeleteSubjectCommandExecute);

    private bool CanDeleteSubjectCommandExecute(object? param)
        => SelectedSubject != null;

    private void OnDeleteSubjectCommandExecuted(object? param)
    {
		if (!_subjectManager.DeleteSubject(SelectedSubject)) return;
        SubjectModels.Remove(SelectedSubject);
    }
}
