using System.Collections.Generic;

namespace WpfApp2.Models;

/// <summary>
/// DTO после редактирования предмета
/// </summary>
/// <param name="Name">Имя измененного предмта</param>
/// <param name="Classes">классы, после изменения</param>
public record SubjectDTO(string Name, List<int> Classes);