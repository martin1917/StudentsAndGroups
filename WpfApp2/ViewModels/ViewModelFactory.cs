using System;
using WpfApp2.State;

namespace WpfApp2.ViewModels;

public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : BaseViewModel;

public class ViewModelFactory
{
    private readonly CreateViewModel<GroupsStudentsViewModel> _createGroupsStudentsViewModel;

    public ViewModelFactory(CreateViewModel<GroupsStudentsViewModel> createGroupsStudentsViewModel)
	{
        _createGroupsStudentsViewModel = createGroupsStudentsViewModel;
    }

    public BaseViewModel CreateViewModel(ViewModelType viewType)
    {
        switch (viewType)
        {
            case ViewModelType.GroupsStudents:
                return _createGroupsStudentsViewModel();
            default:
                throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
        }
    }
}
