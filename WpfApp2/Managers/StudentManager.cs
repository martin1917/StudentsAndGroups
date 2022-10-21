using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WpfApp2.Data;
using WpfApp2.Entity;
using WpfApp2.Models;
using WpfApp2.Services;

namespace WpfApp2.Managers;

/// <summary> Класс менеджера для СТУДЕНТОВ </summary>
public class StudentManager
{
    private readonly IMapper _mapper;
    private readonly StudentDialogService _studentDialogService;
    private readonly CommonDialogService _commonDialogService;

    /// <summary>
    /// конструктор
    /// </summary>
    /// <param name="mapper">Маппер, для отображения одних сущностей на другие</param>
    /// <param name="studentDialogService">Сервис, который будет показывать диалог для студентов</param>
    /// <param name="commonDialogService">Сервис, который будет показывать общие сообщения в виде MessageBox</param>
    public StudentManager(IMapper mapper,
        StudentDialogService studentDialogService,
        CommonDialogService commonDialogService)
    {
        _mapper = mapper;
        _studentDialogService = studentDialogService;
        _commonDialogService = commonDialogService;
    }

    /// <summary>
    /// Редактирование студента
    /// </summary>
    /// <param name="studentModel">Редактируемый студент</param>
    /// <returns> true - если редактирование успешно; false - иначе</returns>
    public bool Edit(StudentModel studentModel)
    {
        var ctx = ContextFactory.CreateContext();
        var groups = _mapper.Map<List<GroupModel>>(ctx.Groups);

        if (!_studentDialogService.Edit(studentModel, groups))
        {
            return false;
        }

        var entity = _mapper.Map<Student>(studentModel);
        ctx.Entry(entity).State = EntityState.Modified;
        ctx.SaveChanges();

        return true;
    }

    /// <summary>
    /// Создать студента в группк
    /// </summary>
    /// <param name="targetGroup">Группа, в которой будет создан студент</param>
    /// <returns>Созданный студент</returns>
    public StudentModel? CreateIn(GroupModel targetGroup)
    {
        var studentModel = new StudentModel();

        if (!_studentDialogService.Create(studentModel, targetGroup))
        {
            return null;
        }

        var entity = _mapper.Map<Student>(studentModel);
        var ctx = ContextFactory.CreateContext();
        var addedEntity = ctx.Students.Add(entity);
        ctx.SaveChanges();

        studentModel.Id = addedEntity.Entity.Id;
        return studentModel;
    }

    /// <summary>
    /// Удаление студента
    /// </summary>
    /// <param name="studentModel">Удаляемый студент</param>
    /// <returns> true - если удаление успешно; false - иначе</returns>
    public bool Delete(StudentModel studentModel)
    {
        var message = 
            $"Вы точно хотите удалить студента?\n" +
            $"Имя: {studentModel.FirstName}\n" +
            $"Фамилия: {studentModel.SecondName}\n" +
            $"Отчество: {studentModel.Patronymic}\n" +
            $"Дата рождения: {studentModel.BirthDay}\n" +
            $"Группа: {studentModel.GroupModel.Name}\n";

        var caption = "Удалние студента";

        if(!_commonDialogService.ConfirmInformation(message, caption))
        {
            return false;
        }

        var entity = _mapper.Map<Student>(studentModel);
        var ctx = ContextFactory.CreateContext();
        ctx.Entry(entity).State = EntityState.Deleted;
        ctx.SaveChanges();
        return true;
    }
}
