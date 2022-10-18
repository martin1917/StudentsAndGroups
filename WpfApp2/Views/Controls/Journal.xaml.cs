using System.Data;
using System.Linq;
using System.Windows.Controls;
using System;
using WpfApp2.Data;
using WpfApp2.ViewModels;
using WpfApp2.Views.Windows;
using System.Text.RegularExpressions;

namespace WpfApp2.Views.Controls;

public partial class Journal
{
    public Journal()
    {
        InitializeComponent();
    }

    private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var dataGrid = (DataGrid)sender;

        var itemsSource = dataGrid.ItemsSource;
        if (itemsSource == null) return;

        var columnHeader = (dataGrid.CurrentCell.Column.Header as string);
        var item = (DataRowView)dataGrid.CurrentCell.Item;

        var partsColumnHeader = columnHeader
            .Split(" ")
            .Select(i => int.Parse(i))
            .ToList();

        var ctx = ContextFactory.CreateContext();
        var date = new DateOnly(partsColumnHeader[2], partsColumnHeader[1], partsColumnHeader[0]);
        var id = (int)item.Row["ID"];
        var marks = (string)item.Row[columnHeader];
        var student = ctx.Students.First(s => s.Id == id);

        var vm = new JournalEditMarksViewModel(student, date, marks);
        var window = new JournalEditMarks { DataContext = vm };
        if (window.ShowDialog() == false) return;

        item.Row[columnHeader] = vm.Marks;

        var marksString = Regex.Replace(vm.Marks.Trim(), @"\s+", "").Split(",");

        // TODO: добавить удаление старых оценок и добавление новых
    }
}