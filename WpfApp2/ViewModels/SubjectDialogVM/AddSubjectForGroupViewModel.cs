using System.Collections.Generic;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels.SubjectDialogVM;

/// <summary> VM для диалога (добавление предмета для группы) </summary>
public class AddSubjectForGroupViewModel : BaseViewModel
{
    private SubjectModel _selectedSubject;

    public AddSubjectForGroupViewModel(List<SubjectModel> subjectModels, int num)
	{
        SubjectModels = subjectModels;
        Num = num;
    }
    
    /// <summary> предметы </summary>
    public List<SubjectModel> SubjectModels { get; set; }
    
    /// <summary> выбранный предмет </summary>
    public SubjectModel SelectedSubject { get => _selectedSubject; set => Set(ref _selectedSubject, value); }

    /// <summary> номер класса </summary>
    public int Num { get; private set; }
}
