using WpfApp2.ViewModels.Base;

namespace WpfApp2.Models;

/// <summary> Учебный предмет (Отображается на UI) </summary>
public class SubjectModel : BaseViewModel
{
    private int _id;
    private string _name;

    public int Id { get => _id; set => Set(ref _id, value); }

    /// <summary> Название учебного предмета </summary>
    public string Name { get => _name; set => Set(ref _name, value); }
}


