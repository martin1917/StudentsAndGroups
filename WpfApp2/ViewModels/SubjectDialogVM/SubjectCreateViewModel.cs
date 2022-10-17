using System.Collections.Generic;
using System.Linq;
using WpfApp2.Data;
using WpfApp2.Entity;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels.SubjectDialogVM;

public class SubjectCreateViewModel : BaseViewModel
{
    private int subjectId;

	public SubjectCreateViewModel(SubjectModel subject)
	{
        subjectId = subject.Id;
        Name = subject.Name;
        LoadDate();
    }

    private void LoadDate()
    {
        if (subjectId == 0)
        {
            NumGroups = new();
        }
        else
        {
            var ctx = ContextFactory.CreateContext();
            NumGroups = new(ctx.SubjectGroups.Where(s => s.SubjectId == subjectId && s.NumGroup != null).Select(s => s.NumGroup));
        }
    }

	private string _name;
	public string Name { get => _name; set => Set(ref _name, value); }

    private List<int> _numGroups;
    public List<int> NumGroups { get => _numGroups; set => Set(ref _numGroups, value); }
}
