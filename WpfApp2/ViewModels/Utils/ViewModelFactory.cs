using System;
using WpfApp2.State;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels.Utils;

public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : BaseViewModel;

public class ViewModelFactory
{
    private readonly CreateViewModel<GroupsStudentsViewModel> _createGroupsStudentsViewModel;
    private readonly CreateViewModel<AllSubjectsViewModel> _createAllSubjectsViewModel;

    public ViewModelFactory(CreateViewModel<GroupsStudentsViewModel> createGroupsStudentsViewModel,
        CreateViewModel<AllSubjectsViewModel> createAllSubjectsViewModel)
    {
        _createGroupsStudentsViewModel = createGroupsStudentsViewModel;
        _createAllSubjectsViewModel = createAllSubjectsViewModel;
    }

    public BaseViewModel CreateViewModel(ViewModelType viewType)
    {
        switch (viewType)
        {
            case ViewModelType.GroupsStudents:
                return _createGroupsStudentsViewModel();
            case ViewModelType.AllSubjects:
                return _createAllSubjectsViewModel();
            default:
                throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
        }
    }
}
