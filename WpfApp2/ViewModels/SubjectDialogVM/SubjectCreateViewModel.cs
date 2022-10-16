using System.Collections.ObjectModel;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels.SubjectDialogVM;

public class SubjectCreateViewModel : BaseViewModel
{

	public SubjectCreateViewModel(SubjectModel subject)
	{
        Name = subject.Name;
        NumGroups = new();
    }

	private string _name;
	public string Name { get => _name; set => Set(ref _name, value); }

    private ObservableCollection<int> _numGroups;
    public ObservableCollection<int> NumGroups { get => _numGroups; set => Set(ref _numGroups, value); }
}
