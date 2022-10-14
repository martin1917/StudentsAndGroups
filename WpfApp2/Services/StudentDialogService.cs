using System.Linq;
using WpfApp2.Data;
using WpfApp2.Entity;
using WpfApp2.ViewModels;
using WpfApp2.Views.Windows;

namespace WpfApp2.Services;

public class StudentDialogService
{
    private Context _ctx;

    public StudentDialogService(Context ctx)
    {
        _ctx = ctx;
    }

    public bool Edit(Student student)
    {
        var groups = _ctx.Groups.ToList();

        var vm = new StudentEditViewModel(student, groups);
        var window = new StudentEditWindow
        {
            DataContext = vm
        };

        if (window.ShowDialog() == false)
        {
            return false;
        }

        student.FirstName = vm.FirstName;
        student.SecondName = vm.SecondName;
        student.Patronymic = vm.Patronymic;
        student.BirthDay = vm.BirthDay;
        student.Group = vm.Group;

        return true;
    }
}
