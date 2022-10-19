using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Data;
using WpfApp2.Infrastructure.Commands;
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
            Nums = string.Empty;
        }
        else
        {
            var ctx = ContextFactory.CreateContext();

            var numGroup = ctx.SubjectGroups
                .Where(s => s.SubjectId == subjectId && s.NumGroup != null)
                .Select(s => s.NumGroup);

            Nums = string.Join(" ", numGroup);
        }
    }

	private string _name;
	public string Name { get => _name; set => Set(ref _name, value); }

    private string _nums;
    public string Nums { get => _nums; set => Set(ref _nums, value); }

    private List<int> _numGroups;
    public List<int> NumGroups { get => _numGroups; set => Set(ref _numGroups, value); }

    private ICommand _confirmCommand;
    public ICommand ConfirmCommand => _confirmCommand
        ??= new Command(OnConfirmCommandExecuted);

    private void OnConfirmCommandExecuted(object? param)
    {
        var error = string.Empty;

        var namePart = Regex.Replace(Name.Trim(), @"\s+", " ").Split(" ");
        if (namePart.Any(p => !Regex.IsMatch(p, @"[a-zA-Zа-яА-Я]+")))
        {
            error += "Название должно состоять только из слов";
        }

        var parts = Regex.Replace(Nums.Trim(), @"\s+", " ").Split(" ");
        if (parts.Any(p => !int.TryParse(p, out int num) || num < 1 || num > 11))
        {
            error += "• Класс - это число от 1 до 11!!!\n" +
                "Перечислите классы через пробел";
        }

        if (string.IsNullOrEmpty(error))
        {
            NumGroups = parts.Select(i => int.Parse(i)).ToList();
            
            var window = App.CurrentWindow;
            window.DialogResult = true;
            window.Close();
        }
        else
        {
            MessageBox.Show(error, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
