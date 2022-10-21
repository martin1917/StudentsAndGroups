using System.Windows.Input;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.State;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels;

/// <summary> Главная VM </summary>
public class MainViewModel : BaseViewModel
{
	private readonly Navigator _navigator;

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="navigator">Навигатор</param>
    public MainViewModel(Navigator navigator) : base(ViewModelType.Main)
	{
		_navigator = navigator;
		_navigator.StateChanged += Navigator_StateChanged;
    }

	private void Navigator_StateChanged()
	{
		OnPropertyChanged(nameof(CurrentViewModel));
	}

	public override void Dispose()
	{
		_navigator.StateChanged -= Navigator_StateChanged;
		base.Dispose();
	}

	/// <summary> Текущая VM </summary>
    public BaseViewModel CurrentViewModel => _navigator.CurrentViewModel;

    private ICommand _updateCurrentViewModelCommand;
    /// <summary> Команда по обновлению текущей VM </summary>
    public ICommand UpdateCurrentViewModelCommand => 
		_updateCurrentViewModelCommand ??= new Command(OnUpdateCurrentViewModelCommandExecuted, CanUpdateCurrentViewModelCommandExecute);

	private bool CanUpdateCurrentViewModelCommandExecute(object? param)
		=> param is ViewModelType viewType 
			&& viewType != CurrentViewModel?.ViewModelType;

    private void OnUpdateCurrentViewModelCommandExecuted(object? param)
	{
		if (param is ViewModelType viewType)
			_navigator.ChangeState(viewType);
	}
}
