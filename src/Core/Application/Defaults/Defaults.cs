using System;
using System.Collections.Generic;
using Shifty.Domain.Features.Shifts;

namespace Shifty.Application.Defaults;

public static class Defaults
{
    public static List<Shift> GetDefaultShifts() =>
    [
        new Shift
        {
            Name             = "شیفت صبح (8 الی 16)" ,
            Arrive           = new TimeOnly(8 ,  0) ,
            Leave            = new TimeOnly(16 , 0) ,
            GraceLateArrival = TimeSpan.FromMinutes(10) ,
            GraceEarlyLeave  = TimeSpan.FromMinutes(5) ,
            Divisions        = []
        } ,

        new Shift
        {
            Name             = "شیفت صبح (8 الی 17)" ,
            Arrive           = new TimeOnly(8 ,  0) ,
            Leave            = new TimeOnly(17 , 0) ,
            GraceLateArrival = TimeSpan.FromMinutes(10) ,
            GraceEarlyLeave  = TimeSpan.FromMinutes(5) ,
            Divisions        = []
        } ,

        new Shift
        {
            Name             = "شیفت صبح (9 الی 18)" ,
            Arrive           = new TimeOnly(9 ,  0) ,
            Leave            = new TimeOnly(18 , 0) ,
            GraceLateArrival = TimeSpan.FromMinutes(10) ,
            GraceEarlyLeave  = TimeSpan.FromMinutes(5) ,
            Divisions        = []
        } ,
    ];



}