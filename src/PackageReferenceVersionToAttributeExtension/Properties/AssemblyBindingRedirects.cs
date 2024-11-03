// <copyright file="AssemblyBindingRedirects.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

using Microsoft.VisualStudio.Shell;

[assembly: ProvideBindingRedirection(
    AssemblyName = "Microsoft.Extensions.DependencyInjection",
    OldVersionLowerBound = "0.0.0.0",
    OldVersionUpperBound = "8.0.0.0",
    NewVersion = "8.0.0.0")]