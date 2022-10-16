using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Data;
using WpfApp2.Models;
using WpfApp2.ViewModels.StudentDialogVM;
using WpfApp2.Views.Windows.StudentDialogs;

namespace WpfApp2.Services;

public class StudentDialogService
{
    private IMapper _mapper;
    private CommonDialogService _commonDialogService;

    public StudentDialogService(IMapper mapper, CommonDialogService commonDialogService)
    {
        _mapper = mapper;
        _commonDialogService = commonDialogService;
    }

    public bool Edit(StudentModel student)
    {
        var context = ContextFactory.CreateContext();
        var groups = _mapper.Map<List<GroupModel>>(context.Groups.AsNoTracking().ToList());
        var vm = new StudentEditViewModel(student, groups);
        var window = new StudentEditWindow
        {
            DataContext = vm
        };

        if (window.ShowDialog() == false)
        {
            return false;
        }

        student.FirstName = vm.FirstName;
        student.SecondName = vm.SecondName;
        student.Patronymic = vm.Patronymic;
        student.BirthDay = DateOnly.FromDateTime(vm.BirthDay);
        student.GroupModel = vm.GroupModel;
        student.GroupId = vm.GroupModel.Id;
        return true;
    }

    public bool Create(StudentModel student, GroupModel group)
    {
        var vm = new StudentCreateViewModel(group);
        var window = new StudentCreateWindow
        {
            DataContext = vm
        };

        if (window.ShowDialog() == false)
        {
            return false;
        }

        student.FirstName = vm.FirstName;
        student.SecondName = vm.SecondName;
        student.Patronymic = vm.Patronymic;
        student.BirthDay = DateOnly.FromDateTime(vm.BirthDay);
        student.GroupModel = vm.GroupModel;
        student.GroupId = vm.GroupModel.Id;
        return true;
    }
}
