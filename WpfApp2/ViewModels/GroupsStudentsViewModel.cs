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
    private IMapper _mapper;

    public GroupsStudentsViewModel(
        Context ctx, 
        StudentDialogService studentDialogService,
        IMapper mapper) : base(ViewModelType.GroupsStudents)
    {
        _ctx = ctx;
        _studentDialogService = studentDialogService;
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

        if (SelectedGroup!.Id != SelectedStudent!.GroupId)
        {
            var group = Groups.First(g => g.Id == SelectedStudent!.GroupId);
            group.StudentModels.Add(SelectedStudent);
            SelectedGroup.StudentModels.Remove(SelectedStudent);
        }
    }
}
