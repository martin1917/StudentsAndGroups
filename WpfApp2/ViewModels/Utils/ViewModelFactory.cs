using System;
using WpfApp2.State;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels.Utils;

public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : BaseViewModel;

public class ViewModelFactory
{
    private readonly CreateViewModel<GroupsStudentsViewModel> _createGroupsStudentsViewModel;
    private readonly CreateViewModel<AllSubjectsViewModel> _createAllSubjectsViewModel;
    private readonly CreateViewModel<SubjectForGroupViewModel> _createSubjectForGroupViewModel;

    public ViewModelFactory(CreateViewModel<GroupsStudentsViewModel> createGroupsStudentsViewModel,
        CreateViewModel<AllSubjectsViewModel> createAllSubjectsViewModel,
        CreateViewModel<SubjectForGroupViewModel> createSubjectForGroupViewModel)
    {
        _createGroupsStudentsViewModel = createGroupsStudentsViewModel;
        _createAllSubjectsViewModel = createAllSubjectsViewModel;
        _createSubjectForGroupViewModel = createSubjectForGroupViewModel;
    }

    public BaseViewModel CreateViewModel(ViewModelType viewType)
    {
        switch (viewType)
        {
            case ViewModelType.GroupsStudents:
                return _createGroupsStudentsViewModel();
            case ViewModelType.AllSubjects:
                return _createAllSubjectsViewModel();
            case ViewModelType.SubjectsForGroup:
                return _createSubjectForGroupViewModel();
            default:
                throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
        }
    }
}
