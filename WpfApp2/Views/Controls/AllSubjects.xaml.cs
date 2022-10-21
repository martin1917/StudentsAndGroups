using System.Windows.Controls;
using System.Windows.Data;
using WpfApp2.Models;

namespace WpfApp2.Views.Controls;

/// <summary> Представление со всеми предметами </summary>
public partial class AllSubjects
{
    /// <summary> Конструктор </summary>
    public AllSubjects()
    {
        InitializeComponent();
    }

    private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
    {
        if (e.Item is not SubjectModel subject) return;
        if (subject.Name == null) return;

        var filter = filterText.Text;
        if(string.IsNullOrEmpty(filter)) return;

        if (subject.Name.Contains(filter, System.StringComparison.OrdinalIgnoreCase)) return;

        e.Accepted = false;
    }

    private void filterText_TextChanged(object sender, TextChangedEventArgs e)
    {
        var collection = (CollectionViewSource)mainGrid.FindResource("subjectCollection");
        collection.View.Refresh();
    }
}
