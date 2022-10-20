using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Data;
using WpfApp2.Entity;
using WpfApp2.Models;
using WpfApp2.Services;

namespace WpfApp2.Managers;

/// <summary> Класс менеджера для ГРУПП </summary>
public class GroupManager
{
    private readonly IMapper _mapper;
    private readonly GroupDialogService _groupDialogService;
    private readonly CommonDialogService _commonDialogService;

    public GroupManager(IMapper mapper, 
        GroupDialogService groupDialogService,
        CommonDialogService commonDialogService)
    {
        _mapper = mapper;
        _groupDialogService = groupDialogService;
        _commonDialogService = commonDialogService;
    }

    /// <summary>
    /// Редактирование группы
    /// </summary>
    /// <param name="groupModel">Редактируемая группа</param>
    /// <returns> true - если редактирование успешно; false - иначе</returns>
    public bool Edit(GroupModel groupModel)
    {
        if (!_groupDialogService.Edit(groupModel))
        {
            return false;
        }

        var entity = _mapper.Map<Group>(groupModel);
        var ctx = ContextFactory.CreateContext();
        ctx.Entry(entity).State = EntityState.Modified;
        ctx.SaveChanges();
        return true;
    }

    /// <summary>
    /// Создание группы
    /// </summary>
    /// <returns> Созданная группа </returns>
    public GroupModel? Create()
    {
        var groupModel = new GroupModel();
        if (!_groupDialogService.Edit(groupModel))
        {
            return null;
        }

        var entity = _mapper.Map<Group>(groupModel);
        var ctx = ContextFactory.CreateContext();
        var addedGroup = ctx.Groups.Add(entity);
        ctx.SaveChanges();

        groupModel.Id = addedGroup.Entity.Id;
        return groupModel;
    }

    /// <summary>
    /// Удаление группы
    /// </summary>
    /// <param name="groupModel">Удаляемая группа</param>
    /// <returns> true - если удаление успешно; false - иначе</returns>
    public bool Delete(GroupModel groupModel)
    {
        var message =
            $"Вы точно хотите удалить группу ?\n" +
            $"Имя: {groupModel.Name}\n" +
            $"ВСЕ СТУДЕНТЫ БУДУТ УДАЛЕНЫ ИЗ ЭТОЙ ГРУППЫ";

        var caption = "Удаление группы";

        if(!_commonDialogService.ConfirmInformation(message, caption))
        {
            return false;
        }

        var ctx = ContextFactory.CreateContext();
        if (groupModel.StudentModels != null)
        {
            foreach (var studentModel in groupModel.StudentModels)
            {
                var entityStudent = _mapper.Map<Student>(studentModel);
                ctx.Entry(entityStudent).State = EntityState.Deleted;
            }
        }

        var entityGroup = _mapper.Map<Group>(groupModel);
        ctx.Entry(entityGroup).State = EntityState.Deleted;
        ctx.SaveChanges();
        return true;
    }
}
