// Global using directives

global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
global using FluentValidation;
global using Mapster;
global using MediatR;
global using Microsoft.AspNetCore.Http;
global using Swashbuckle.AspNetCore.Filters;
global using Microsoft.Extensions.Localization;
global using SmartAttendance.Application.Abstractions;
global using SmartAttendance.Application.Commons.MediaFiles.Requests;
global using SmartAttendance.Application.Features.Calendars.Request.Commands.CreateReminder;
global using SmartAttendance.Application.Interfaces.Base;
global using SmartAttendance.Common.General.Enums.FileType;
global using SmartAttendance.Common.Utilities.InjectionHelpers;
global using SmartAttendance.Domain.Common;
global using SmartAttendance.Domain.HubFiles;
global using SmartAttendance.Domain.Setting;
global using SmartAttendance.Domain.Tenants;
global using SmartAttendance.Domain.Users;
global using SmartAttendance.Domain.ValueObjects;