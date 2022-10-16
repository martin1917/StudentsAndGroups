using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfApp2.Data;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.Models;
using WpfApp2.Services;
using WpfApp2.State;

namespace WpfApp2.ViewModels;

public class GroupsStudentsViewModel : BaseViewModel
{
	private Context _ctx;
    private StudentDialogService _studentDialogService;
    private CommonDialogService _commonDialogService;
    private IMapper _mapper;

    public GroupsStudentsViewModel(
        Context ctx, 
        StudentDialogService studentDialogService,
        CommonDialogService commonDialogService,
        IMapper mapper) : base(ViewModelType.GroupsStudents)
    {
        _ctx = ctx;
        _studentDialogService = studentDialogService;
        _commonDialogService = commonDialogService;
        _mapper = mapper;

        LoadData();
    }

    // Группы
    public ObservableCollection<GroupModel> Groups { get; } = new();

    // Выбранная группа
    private GroupModel? _selectedGroup;
    public GroupModel? SelectedGroup { get => _selectedGroup; set => Set(ref _selectedGroup, value); }

    // Выбранный студент
    private StudentModel? _selectedStudent;
    public StudentModel? SelectedStudent { get => _selectedStudent; set => Set(ref _selectedStudent, value); }

    private void LoadData()
    {
        var items = _mapper.Map<ObservableCollection<GroupModel>>(_ctx.Groups.Include(g => g.Students));
        Groups.Clear();
        foreach (var item in items)
        {
            Groups.Add(item);
        }
    }

    // редактирование студента
    private ICommand _editStudentCommand;
    public ICommand EditStudentCommand => _editStudentCommand
        ??= new Command(OnEditStudentCommandExecuted, CanEditStudentCommandExecute);

    private bool CanEditStudentCommandExecute(object? arg)
        => SelectedStudent != null;

    private void OnEditStudentCommandExecuted(object? obj)
    {
        if (!_studentDialogService.Edit(SelectedStudent))
        {
            return;
        }

        if (SelectedGroup!.Id != SelectedStudent!.GroupId)
        {
            var group = Groups.First(g => g.Id == SelectedStudent!.GroupId);
            group.StudentModels.Add(SelectedStudent);
            SelectedGroup.StudentModels.Remove(SelectedStudent);
        }
    }

    // создание студента
    private ICommand _createStudentCommand;
    public ICommand CreateStudentCommand => _createStudentCommand
        ??= new Command(OnCreateStudentCommandExecuted, CanCreateStudentCommandExecute);

    private bool CanCreateStudentCommandExecute(object? param)
        => SelectedGroup != null;

    private void OnCreateStudentCommandExecuted(object? param)
    {
        var newStudentModel = new StudentModel();

        if (!_studentDialogService.Create(newStudentModel, SelectedGroup!))
        {
            return;
        }


        var group = Groups.First(g => g.Id == newStudentModel!.GroupId);
        group.StudentModels.Add(newStudentModel);
    }

    // удаление студента
    private ICommand _deleteStudentCommand;
    public ICommand DeleteStudentCommand => _deleteStudentCommand
        ??= new Command(OnDeleteStudentCommandExecuted, CanDeleteStudentCommandExecute);

    private bool CanDeleteStudentCommandExecute(object? param)
        => SelectedStudent != null;

    private void OnDeleteStudentCommandExecuted(object? param)
    {
        if (!_commonDialogService.ConfirmInformation($"Вы точно хотите удалить студента\n" +
            $"Имя: {SelectedStudent.FirstName}\n" +
            $"Фамилия: {SelectedStudent.SecondName}\n" +
            $"Отчество: {SelectedStudent.Patronymic}\n" +
            $"Дата рождения: {SelectedStudent.BirthDay}\n" +
            $"Группа: {SelectedStudent.GroupModel.Name}\n", "Удалние студента"))
        {
            return;
        }

        var group = Groups.First(g => g.Id == SelectedStudent!.GroupId);
        group.StudentModels.Remove(SelectedStudent);
    }
}
