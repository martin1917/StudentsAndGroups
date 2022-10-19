using OfficeOpenXml;
using System.Data;

DataTable table = new();

table.Columns.Add("ID", typeof(int));
table.Columns.Add("Имя", typeof(string));
table.Columns.Add("Зарплата", typeof(decimal));

table.Rows.Add(1, "Maksim", 999m);
table.Rows.Add(2, "Vlad", 500m);
table.Rows.Add(3, "Denis", 9000m);
table.Rows.Add(4, "Keril", 4000m);
table.Rows.Add(5, "Anton", 1500m);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var package = new ExcelPackage();
var sheet = package.Workbook.Worksheets.Add("Sheet");

var rowHeader = 2;
var colHeader = 2;

// column headers
for (int i = 0; i < table.Columns.Count; i++)
{
    var col = table.Columns[i];
    sheet.Cells[rowHeader, colHeader + i].Value = col.ColumnName;
}

for (int i = 0; i < table.Rows.Count; i++)
{
    var row = table.Rows[i];
    int column = colHeader;
    for (int j = 0; j < row.ItemArray.Length; j++)
    {
        var value = row.ItemArray[j];
        sheet.Cells[rowHeader + i + 1, column + j].Value = value!.ToString();
    }
}

byte[] bytes = package.GetAsByteArray();

File.WriteAllBytes("Some.xlsx", bytes);