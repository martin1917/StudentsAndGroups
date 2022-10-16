using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;
using WpfApp2.Data;
using WpfApp2.Entity;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.Models;
using WpfApp2.Services;
using WpfApp2.State;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels;

public class GroupsStudentsViewModel : BaseViewModel
{
    private StudentDialogService _studentDialogService;
    private GroupDialogService _groupDialogService;
    private CommonDialogService _commonDialogService;
    private IMapper _mapper;

    public GroupsStudentsViewModel(
        StudentDialogService studentDialogService,
        GroupDialogService groupDialogService,
        CommonDialogService commonDialogService,
        IMapper mapper) : base(ViewModelType.GroupsStudents)
    {
        _studentDialogService = studentDialogService;
        _groupDialogService = groupDialogService;
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
        var context = ContextFactory.CreateContext();
        var items = _mapper.Map<ObservableCollection<GroupModel>>(context.Groups.AsNoTracking().Include(g => g.Students));
        Groups.Clear();
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

    private bool CanEditStudentCommandExecute(object? arg)
        => SelectedStudent != null;

    private void OnEditStudentCommandExecuted(object? obj)
    {
        if (!_studentDialogService.Edit(SelectedStudent))
        {
            return;
        }

        var student = _mapper.Map<Student>(SelectedStudent);
        var context = ContextFactory.CreateContext();
        context.Entry(student).State = EntityState.Modified;
        context.SaveChanges();

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

        var student = _mapper.Map<Student>(newStudentModel);
        var context = ContextFactory.CreateContext();
        var addedStudent = context.Students.Add(student);
        context.SaveChanges();

        newStudentModel.Id = addedStudent.Entity.Id;
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
        if (!_commonDialogService.ConfirmInformation($"Вы точно хотите удалить студента ?\n" +
            $"Имя: {SelectedStudent.FirstName}\n" +
            $"Фамилия: {SelectedStudent.SecondName}\n" +
            $"Отчество: {SelectedStudent.Patronymic}\n" +
            $"Дата рождения: {SelectedStudent.BirthDay}\n" +
            $"Группа: {SelectedStudent.GroupModel.Name}\n", "Удалние студента"))
        {
            return;
        }

        var student = _mapper.Map<Student>(SelectedStudent);
        var context = ContextFactory.CreateContext();
        context.Entry(student).State = EntityState.Deleted;
        context.SaveChanges();

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
        if (!_groupDialogService.Edit(SelectedGroup))
        {
            return;
        }

        var group = _mapper.Map<Group>(SelectedGroup);
        var context = ContextFactory.CreateContext();
        context.Entry(group).State = EntityState.Modified;
        context.SaveChanges();
    }

    // Создание группы
    private ICommand _createGroupCommand;
    public ICommand CreateGroupCommand => _createGroupCommand
        ??= new Command(OnCreateGroupCommandExecuted);

    private void OnCreateGroupCommandExecuted(object? param)
    {
        var newGroupModel = new GroupModel();
        if (!_groupDialogService.Edit(newGroupModel))
        {
            return;
        }

        var group = _mapper.Map<Group>(newGroupModel);
        var context = ContextFactory.CreateContext();
        var addedGroup = context.Groups.Add(group);
        context.SaveChanges();

        newGroupModel.Id = addedGroup.Entity.Id;

        Groups.Add(newGroupModel);
    }

    // удаление группы
    private ICommand _deleteGroupCommand;
    public ICommand DeleteGroupCommand => _deleteGroupCommand
        ??= new Command(OnDeleteGroupCommandExecuted, CanDeleteGroupCommandExecute);

    private bool CanDeleteGroupCommandExecute(object? param)
        => SelectedGroup != null;

    private void OnDeleteGroupCommandExecuted(object? param)
    {
        if (!_commonDialogService.ConfirmInformation($"Вы точно хотите удалить группу ?\n" +
            $"Имя: {SelectedGroup.Name}\n" +
            $"ВСЕ СТУДЕНТЫ БУДУТ УДАЛЕНЫ ИЗ ЭТОЙ ГРУППЫ", "Удаление группы"))
        {
            return;
        }

        var context = ContextFactory.CreateContext();

        if (SelectedGroup.StudentModels != null)
        {
            foreach (var studentModel in SelectedGroup.StudentModels)
            {
                var student = _mapper.Map<Student>(studentModel);
                context.Entry(student).State = EntityState.Deleted;
            }
        }

        var group = _mapper.Map<Group>(SelectedGroup);
        context.Entry(group).State = EntityState.Deleted;
        context.SaveChanges();

        Groups.Remove(SelectedGroup);
    }
    #endregion
}
