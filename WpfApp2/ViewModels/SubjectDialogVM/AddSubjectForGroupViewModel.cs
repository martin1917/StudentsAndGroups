using System.Collections.Generic;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels.SubjectDialogVM;

public class AddSubjectForGroupViewModel : BaseViewModel
{
    public AddSubjectForGroupViewModel(List<SubjectModel> subjectModels, int num)
	{
        SubjectModels = subjectModels;
        Num = num;
    }
    
    public List<SubjectModel> SubjectModels { get; set; }

    private SubjectModel _selectedSubject;
    public SubjectModel SelectedSubject { get => _selectedSubject; set => Set(ref _selectedSubject, value); }

    public int Num { get; private set; }
}
