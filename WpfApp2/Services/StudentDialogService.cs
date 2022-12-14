using AutoMapper;
using System;
using System.Collections.Generic;
using WpfApp2.Data;
using WpfApp2.Models;
using WpfApp2.ViewModels.StudentDialogVM;
using WpfApp2.Views.Windows.StudentDialogs;

namespace WpfApp2.Services;

/// <summary> Диалоговый сервис для УЧЕНИКОВ </summary>
public class StudentDialogService
{
    private IMapper _mapper;

    public StudentDialogService(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Отображение диалогового окна - для редактирование
    /// </summary>
    /// <param name="student"></param>
    /// <param name="groups"></param>
    /// <returns>true - если редактирование удачно; false - иначе</returns>
    public bool Edit(StudentModel student)
    {
        var ctx = ContextFactory.CreateContext();
        var groups = _mapper.Map<List<GroupModel>>(ctx.Groups);

        var vm = new StudentEditViewModel(student, groups);
        var window = new StudentEditWindow { DataContext = vm };

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

    /// <summary>
    /// Отображение диалогового окна - для создания
    /// </summary>
    /// <param name="student"></param>
    /// <param name="group"></param>
    /// <returns>true - если создание удачно; false - иначе</returns>
    public bool Create(StudentModel student, GroupModel group)
    {
        var vm = new StudentCreateViewModel(group);
        var window = new StudentCreateWindow { DataContext = vm };

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
