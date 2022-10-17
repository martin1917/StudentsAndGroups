using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WpfApp2.Data;
using WpfApp2.Entity;
using WpfApp2.Models;
using WpfApp2.Services;

namespace WpfApp2.Managers;

public class SubjectManager
{
    private readonly IMapper _mapper;
    private readonly SubjectDialogService _subjectDialogService;
    private readonly CommonDialogService _commonDialogService;

    public SubjectManager(IMapper mapper,
        SubjectDialogService subjectDialogService,
        CommonDialogService commonDialogService)
    {
        _mapper = mapper;
        _subjectDialogService = subjectDialogService;
        _commonDialogService = commonDialogService;
    }

    public SubjectModel? Create()
    {
        var subjectModel = new SubjectModel();
        var result = _subjectDialogService.Edit(subjectModel);

        if (result == null)
        {
            return null;
        }

        subjectModel.Name = result.Name;

        var ctx = ContextFactory.CreateContext();
        var entity = _mapper.Map<Subject>(subjectModel);
        var added = ctx.Subjects.Add(entity);
        subjectModel.Id = added.Entity.Id;

        foreach (var numClass in result.Classes)
        {
            ctx.SubjectGroups.Add(new SubjectGroup
            {
                NumGroup = numClass,
                Subject = entity
            });
        }

        ctx.SaveChanges();
        return subjectModel;
    }

    public bool Edit(SubjectModel subjectModel)
    {
        var result = _subjectDialogService.Edit(subjectModel);

        if(result == null)
        {
            return false;
        }

        var ctx = ContextFactory.CreateContext();

        var alreadyExistNums = ctx.SubjectGroups
            .Where(s => s.SubjectId == subjectModel.Id && s.NumGroup != null)
            .Select(s => s.NumGroup).ToList();

        var prevNums = alreadyExistNums.Except(result.Classes);
        var newNums = result.Classes.Except(alreadyExistNums);

        subjectModel.Name = result.Name;
        var entity = _mapper.Map<Subject>(subjectModel);
        ctx.Entry(entity).State = EntityState.Modified;

        foreach (var prevNum in prevNums)
        {
            var prevRow = ctx.SubjectGroups.First(sg => sg.SubjectId == entity.Id && sg.NumGroup == prevNum);
            ctx.Entry(prevRow).State = EntityState.Deleted;
        }

        foreach (var newNum in newNums)
        {
            ctx.SubjectGroups.Add(new SubjectGroup
            {
                NumGroup = newNum,
                Subject = entity
            });
        }

        ctx.SaveChanges();
        return true;
    }

    public bool DeleteSubject(SubjectModel subjectModel)
    {
        var message =
            $"Вы точно хотите удалить предмет ?\n" +
            $"Предмет: {subjectModel.Name}\n";

        var caption = "Удаление группы";

        if (!_commonDialogService.ConfirmInformation(message, caption))
        {
            return false;
        }

        var ctx = ContextFactory.CreateContext();
        var entity = _mapper.Map<Subject>(subjectModel);
        
        var SubjectGroups = ctx.SubjectGroups.Where(sg => sg.SubjectId == subjectModel.Id);

        foreach (var subjectGroup in SubjectGroups)
        {
            ctx.Entry(subjectGroup).State = EntityState.Deleted;
        }

        ctx.Entry(entity).State = EntityState.Deleted;
        ctx.SaveChanges();
        return true;
    }

    public SubjectModel? AddSubjectForGroup(int numGroup)
    {
        var result = _subjectDialogService.AddSubject(numGroup);

        if (result == null)
        {
            return null;
        }

        var ctx = ContextFactory.CreateContext();
        ctx.SubjectGroups.Add(new SubjectGroup
        {
            NumGroup = numGroup,
            SubjectId = result.Id
        });
        ctx.SaveChanges();

        return result;
    }

    public IEnumerable<Subject> GetSubjectsForGroup(int numGroup)
    {
        var ctx = ContextFactory.CreateContext();

        var subjects = ctx.SubjectGroups
            .Where(sg => sg.NumGroup == numGroup)
            .Include(sg => sg.Subject)
            .Select(sg => sg.Subject);

        return subjects;
    }

    public bool DeleteSubjectForGroup(SubjectModel subjectModel, int NumGroup)
    {
        var ctx = ContextFactory.CreateContext();

        var message = 
            $"Вы точно хотите удалить предмет: " +
            $"{subjectModel.Name} " +
            $"для {NumGroup} класса ?";

        var caption = "Удаление предмета для группы";

        if (!_commonDialogService.ConfirmInformation(message, caption))
        {
            return false;
        }

        var row = ctx.SubjectGroups
            .First(sg => sg.NumGroup == NumGroup && sg.SubjectId == subjectModel.Id);

        ctx.Entry(row).State = EntityState.Deleted;
        ctx.SaveChanges();
        
        return true;
    }
}
