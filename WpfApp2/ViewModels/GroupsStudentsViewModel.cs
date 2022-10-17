using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfApp2.Data;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.Managers;
using WpfApp2.Models;
using WpfApp2.State;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels;

public class GroupsStudentsViewModel : BaseViewModel
{
    private StudentManager _studentManager;
    private GroupManager _groupManager;
    private IMapper _mapper;

    public GroupsStudentsViewModel(
        StudentManager studentManager,
        GroupManager groupManager,
        IMapper mapper) : base(ViewModelType.GroupsStudents)
    {
        _studentManager = studentManager;
        _groupManager = groupManager;
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
        var context = ContextFactory.CreateContext();
        var items = _mapper.Map<ObservableCollection<GroupModel>>(context.Groups.Include(g => g.Students));
        foreach (var item in items)
        {
            Groups.Add(item);
        }
    }

    #region Команды по изменение студентов
    // редактирование студента
    private ICommand _editStudentCommand;
    public ICommand EditStudentCommand => _editStudentCommand
        ??= new Command(OnEditStudentCommandExecuted, CanEditStudentCommandExecute);

    private bool CanEditStudentCommandExecute(object? arg) => SelectedStudent != null;

    private void OnEditStudentCommandExecuted(object? obj)
    {
        if (!_studentManager.Edit(SelectedStudent))
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

    private bool CanCreateStudentCommandExecute(object? param) => SelectedGroup != null;

    private void OnCreateStudentCommandExecuted(object? param)
    {
        var newStudentModel = _studentManager.CreateIn(SelectedGroup);
        if (newStudentModel == null)
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

    private bool CanDeleteStudentCommandExecute(object? param) => SelectedStudent != null;

    private void OnDeleteStudentCommandExecuted(object? param)
    {
        if (!_studentManager.Delete(SelectedStudent))
        {
            return;
        }

        var group = Groups.First(g => g.Id == SelectedStudent!.GroupId);
        group.StudentModels.Remove(SelectedStudent);
    }
    #endregion

    #region Команды по изменение групп
    // редактирование группы
    private ICommand _editGroupCommand;
    public ICommand EditGroupCommand => _editGroupCommand
        ??= new Command(OnEditGroupCommandExecuted, CanEditGroupCommandExecute);

    private bool CanEditGroupCommandExecute(object? param)
        => SelectedGroup != null;

    private void OnEditGroupCommandExecuted(object? param)
    {
        _groupManager.Edit(SelectedGroup);
    }

    // Создание группы
    private ICommand _createGroupCommand;
    public ICommand CreateGroupCommand => _createGroupCommand
        ??= new Command(OnCreateGroupCommandExecuted);

    private void OnCreateGroupCommandExecuted(object? param)
    {
        var newGroupModel = _groupManager.Create();
        Groups.Add(newGroupModel);
    }

    // удаление группы
    private ICommand _deleteGroupCommand;
    public ICommand DeleteGroupCommand => _deleteGroupCommand
        ??= new Command(OnDeleteGroupCommandExecuted, CanDeleteGroupCommandExecute);

    private bool CanDeleteGroupCommandExecute(object? param) => SelectedGroup != null;

    private void OnDeleteGroupCommandExecuted(object? param)
    {
        if (!_groupManager.Delete(SelectedGroup))
        {
            return;
        }

        Groups.Remove(SelectedGroup);
    }
    #endregion
}
