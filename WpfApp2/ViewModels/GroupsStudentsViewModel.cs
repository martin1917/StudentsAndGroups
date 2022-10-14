using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApp2.Data;
using WpfApp2.Entity;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.Services;
using WpfApp2.State;

namespace WpfApp2.ViewModels;

public class GroupsStudentsViewModel : BaseViewModel
{
	private Context _ctx;
    private StudentDialogService _studentDialogService;

    public GroupsStudentsViewModel(
        Context ctx, 
        StudentDialogService studentDialogService
        ) : base(ViewModelType.GroupsStudents)
    {
        _ctx = ctx;
        _studentDialogService = studentDialogService;
        LoadData();
    }

    // Группы
    public ObservableCollection<Group> Groups { get; } = new();

    // Выбранная группа
    private Group? _selectedGroup;
    public Group? SelectedGroup { get => _selectedGroup; set => Set(ref _selectedGroup, value); }

    // Выбранный студент
    private Student? _selectedStudent;
    public Student? SelectedStudent { get => _selectedStudent; set => Set(ref _selectedStudent, value); }

    private void LoadData()
    {
        Groups.Clear();
        foreach (var item in _ctx.Groups.Include(g => g.Students))
        {
            Groups.Add(item);
        }
    }

    private ICommand _editStudentCommand;
    public ICommand EditStudentCommand => _editStudentCommand
        ??= new Command(OnEditStudentCommandExecuted, CanEditStudentCommandExecute);

    private bool CanEditStudentCommandExecute(object? arg)
    {
        return SelectedStudent != null;
    }

    private void OnEditStudentCommandExecuted(object? obj)
    {
        if (!_studentDialogService.Edit(SelectedStudent))
        {
            return;
        }

        SelectedStudent.Patronymic = "hello world";
        OnPropertyChanged(nameof(SelectedStudent.Patronymic));

        if (SelectedGroup!.Id != SelectedStudent!.Group.Id)
        {
            SelectedGroup.Students.Remove(SelectedStudent);
            SelectedStudent.Group.Students.Add(SelectedStudent);
        }
    }
}
