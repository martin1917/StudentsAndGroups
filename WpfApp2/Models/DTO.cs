using System.Collections.ObjectModel;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.Models;

public class StudentAndMarkInDate : BaseViewModel
{
    public StudentModel StudentModel { get; set; }

    private string _marks = string.Empty;
    public string Marks { get => _marks; set => Set(ref _marks, value); }

    public StudentAndMarkInDate(StudentModel studentModel)
    {
        StudentModel = studentModel;
    }
}
