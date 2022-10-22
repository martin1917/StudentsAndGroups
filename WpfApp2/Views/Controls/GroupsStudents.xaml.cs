using System.Windows.Data;
using WpfApp2.Models;

namespace WpfApp2.Views.Controls;

/// <summary> Представление с группами с студентами </summary>
public partial class GroupsStudents
{
    /// <summary> Конструктор </summary>
    public GroupsStudents()
    {
        InitializeComponent();
    }

    private void groupFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        var collection = (CollectionViewSource)groupDockPanel.FindResource("groupCollection");
        collection.View.Refresh();
    }

    private void CollectionViewSource_Filter(object sender, System.Windows.Data.FilterEventArgs e)
    {
        if (e.Item is not GroupModel group) return;
        if (group.Name == null) return;

        var filter = groupFilter.Text;
        if (string.IsNullOrEmpty(filter)) return;

        if(group.Name.Contains(filter, System.StringComparison.OrdinalIgnoreCase)) return;
        
        e.Accepted = false;
    }
}
