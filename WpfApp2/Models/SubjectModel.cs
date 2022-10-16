using WpfApp2.ViewModels.Base;

namespace WpfApp2.Models;

public class SubjectModel : BaseViewModel
{
    private int _id;
    public int Id { get => _id; set => Set(ref _id, value); }

    private string _name;
    public string Name { get => _name; set => Set(ref _name, value); }
}


