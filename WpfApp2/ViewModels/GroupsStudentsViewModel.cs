using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using WpfApp2.Data;
using WpfApp2.Entity;
using WpfApp2.State;

namespace WpfApp2.ViewModels;

public class GroupsStudentsViewModel : BaseViewModel
{
	private Context _ctx;

    public GroupsStudentsViewModel(Context ctx) : base(ViewModelType.GroupsStudents)
    {
        _ctx = ctx;
        LoadData();
    }

    // Группы
    public ObservableCollection<Group> Groups { get; } = new ObservableCollection<Group>();

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
}
