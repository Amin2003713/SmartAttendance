using System;
using System.Collections.Generic;
using Shifty.Common.Utilities;
using Shifty.Domain.Features.Divisions;
using Shifty.Domain.Features.Setting;
using Shifty.Domain.Features.Shifts;

namespace Shifty.Domain.Defaults;

public static class Defaults
{
    public static Division GetDivisions()
    {
        return new Division
        {
            Name     = "هیات مدیره" ,
            ParentId = null , // Top-level division
            Children = new List<Division>
            {
                new Division
                {
                    Name = "مدیر عامل" ,
                    Children = new List<Division>
                    {
                        new Division
                        {
                            Name = "معاونت اجرایی" ,
                            Children = new List<Division>
                            {
                                new Division
                                {
                                    Name = "واحد اجرایی" ,
                                } ,
                                new Division
                                {
                                    Name = "واحد پشتیبانی" ,
                                } ,
                            } ,
                        } ,
                        new Division
                        {
                            Name = "معاونت فنی" ,
                            Children = new List<Division>
                            {
                                new Division
                                {
                                    Name = "واحد فنی" ,
                                } ,
                                new Division
                                {
                                    Name = "واحد توسعه" ,
                                } ,
                                new Division
                                {
                                    Name = "واحد تحقیق و توسعه" ,
                                } ,
                            } ,
                        } ,
                        new Division
                        {
                            Name = "معاونت مالی و اداری" ,
                            Children = new List<Division>
                            {
                                new Division
                                {
                                    Name = "واحد مالی" ,
                                } ,
                                new Division
                                {
                                    Name = "واحد منابع انسانی" ,
                                } ,
                                new Division
                                {
                                    Name = "واحد حقوقی" ,
                                } ,
                            } ,
                        } ,
                    } ,
                } ,
            } ,
        };
    }

    public static Setting GetSettings()
    {
        return new Setting()
        {
            Flags = (SettingFlags.CompanyEnabled | SettingFlags.InitialStepper) ,
        };
    }

    public static List<Shift> GetDefaultShifts()
    {
        return
        [
            new Shift
            {
                Name             = "شیفت صبح (8 الی 16)" ,
                Arrive           = new TimeOnly(8 ,  0) ,
                Leave            = new TimeOnly(16 , 0) ,
                GraceLateArrival = TimeSpan.FromMinutes(10) ,
                GraceEarlyLeave  = TimeSpan.FromMinutes(5) ,
                Divisions        = [] ,
            } ,

            new Shift
            {
                Name             = "شیفت صبح (8 الی 17)" ,
                Arrive           = new TimeOnly(8 ,  0) ,
                Leave            = new TimeOnly(17 , 0) ,
                GraceLateArrival = TimeSpan.FromMinutes(10) ,
                GraceEarlyLeave  = TimeSpan.FromMinutes(5) ,
                Divisions        = [] ,
            } ,

            new Shift
            {
                Name             = "شیفت صبح (9 الی 18)" ,
                Arrive           = new TimeOnly(9 ,  0) ,
                Leave            = new TimeOnly(18 , 0) ,
                GraceLateArrival = TimeSpan.FromMinutes(10) ,
                GraceEarlyLeave  = TimeSpan.FromMinutes(5) ,
                Divisions        = [] ,
            } ,
        ];
    }
}