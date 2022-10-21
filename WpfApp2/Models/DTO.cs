using System.Collections.ObjectModel;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.Models;

/// <summary> Оценки ученика за определенную дату </summary>
public class StudentAndMarkInDate : BaseViewModel
{
    /// <summary> Ученик </summary>
    public StudentModel StudentModel { get; set; }

    private string _marks = string.Empty;
    /// <summary> Оценки ученика в строков формате </summary>
    public string Marks { get => _marks; set => Set(ref _marks, value); }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="studentModel">Информация о студент</param>
    public StudentAndMarkInDate(StudentModel studentModel)
    {
        StudentModel = studentModel;
    }
}
