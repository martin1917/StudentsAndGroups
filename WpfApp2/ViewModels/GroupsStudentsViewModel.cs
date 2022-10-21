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

/// <summary> VM со студентами и группами </summary>
public class GroupsStudentsViewModel : BaseViewModel
{
    private StudentManager _studentManager;
    private GroupManager _groupManager;

    private IMapper _mapper;

    private GroupModel? _selectedGroup;
    private StudentModel? _selectedStudent;

    private ICommand _editStudentCommand;
    private ICommand _createStudentCommand;
    private ICommand _deleteStudentCommand;
    private ICommand _editGroupCommand;
    private ICommand _createGroupCommand;
    private ICommand _deleteGroupCommand;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="studentManager">Менеджер для работы со студентами</param>
    /// <param name="groupManager">Менеджер для работы с группами</param>
    /// <param name="mapper">Маппер, для отображения одних сущностей на другие</param>
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

    /// <summary> Группы </summary>
    public ObservableCollection<GroupModel> Groups { get; } = new();

    /// <summary> Выбранная группа </summary>
    public GroupModel? SelectedGroup { get => _selectedGroup; set => Set(ref _selectedGroup, value); }

    /// <summary> Выбранный студент </summary>
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

    /// <summary> редактирование студента </summary>
    public ICommand EditStudentCommand => 
        _editStudentCommand ??= new Command(OnEditStudentCommandExecuted, CanEditStudentCommandExecute);

    private bool CanEditStudentCommandExecute(object? arg) 
        => SelectedStudent != null;

    private void OnEditStudentCommandExecuted(object? obj)
    {
        if (!_studentManager.Edit(SelectedStudent)) return;

        if (SelectedGroup!.Id != SelectedStudent!.GroupId)
        {
            var group = Groups.First(g => g.Id == SelectedStudent!.GroupId);
            group.StudentModels.Add(SelectedStudent);
            SelectedGroup.StudentModels.Remove(SelectedStudent);
        }
    }
    
    /// <summary> создание студента </summary>
    public ICommand CreateStudentCommand => 
        _createStudentCommand ??= new Command(OnCreateStudentCommandExecuted, CanCreateStudentCommandExecute);

    private bool CanCreateStudentCommandExecute(object? param) 
        => SelectedGroup != null;

    private void OnCreateStudentCommandExecuted(object? param)
    {
        var newStudentModel = _studentManager.CreateIn(SelectedGroup);
        if (newStudentModel == null) return;

        var group = Groups.First(g => g.Id == newStudentModel!.GroupId);
        group.StudentModels.Add(newStudentModel);
    }
        
    /// <summary> удаление студента </summary>
    public ICommand DeleteStudentCommand => 
        _deleteStudentCommand ??= new Command(OnDeleteStudentCommandExecuted, CanDeleteStudentCommandExecute);

    private bool CanDeleteStudentCommandExecute(object? param) 
        => SelectedStudent != null;

    private void OnDeleteStudentCommandExecuted(object? param)
    {
        if (!_studentManager.Delete(SelectedStudent)) return;

        var group = Groups.First(g => g.Id == SelectedStudent!.GroupId);
        group.StudentModels.Remove(SelectedStudent);
    }
    
    /// <summary> редактирование группы </summary>
    public ICommand EditGroupCommand => 
        _editGroupCommand ??= new Command(OnEditGroupCommandExecuted, CanEditGroupCommandExecute);

    private bool CanEditGroupCommandExecute(object? param)
        => SelectedGroup != null;

    private void OnEditGroupCommandExecuted(object? param)
    {
        _groupManager.Edit(SelectedGroup);
    }
    
    /// <summary> Создание группы </summary>
    public ICommand CreateGroupCommand => 
        _createGroupCommand ??= new Command(OnCreateGroupCommandExecuted);

    private void OnCreateGroupCommandExecuted(object? param)
    {
        var newGroupModel = _groupManager.Create();
        Groups.Add(newGroupModel);
    }
    
    /// <summary> удаление группы </summary>
    public ICommand DeleteGroupCommand => 
        _deleteGroupCommand ??= new Command(OnDeleteGroupCommandExecuted, CanDeleteGroupCommandExecute);

    private bool CanDeleteGroupCommandExecute(object? param) 
        => SelectedGroup != null;

    private void OnDeleteGroupCommandExecuted(object? param)
    {
        if (!_groupManager.Delete(SelectedGroup)) return;

        Groups.Remove(SelectedGroup);
    }
}
