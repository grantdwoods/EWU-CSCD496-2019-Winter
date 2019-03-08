
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Do not directly await a Task", 
    Justification = "No SynchronizationContext in .net core")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721:Property names should not match get methods", 
    Justification = "Method naming conventions are meant to match web API expected HTTP verb GET, not a usual c# getter method. " +
        "Naming also assists Swagger documentation(NSwag).")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:Uri properties should not be strings", 
    Justification = "EF will not map URI to string")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", 
    Justification = "Class is meant to be mutable (mostly because of automapper)", Scope = "member", Target = "~P:SecretSanta.Api.ViewModels.GroupViewModel.GroupUsers")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1506:Avoid excessive class coupling", 
    Justification = "DI Container is bound to have a multitude of dependancies.", Scope = "member", Target = "~M:SecretSanta.Api.Startup.ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1060:Move pinvokes to native methods class", 
    Justification = "It seems fixes for this warning cause errors that prevent the class from functioning.", Scope = "type", Target = "~T:SecretSanta.Api.Models.CurrentDirectoryHelpers")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", 
    Justification = "Class Program contains Main method.", Scope = "type", Target = "~T:SecretSanta.Api.Program")]
