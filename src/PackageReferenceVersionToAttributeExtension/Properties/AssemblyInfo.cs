﻿// <copyright file="AssemblyInfo.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

using System.Reflection;
using System.Runtime.CompilerServices;
using PackageReferenceVersionToAttributeExtension;

[assembly: AssemblyTitle(Vsix.Name)]
[assembly: AssemblyDescription(Vsix.Description)]
[assembly: AssemblyCompany(Vsix.Author)]
[assembly: AssemblyProduct(Vsix.Name)]
[assembly: AssemblyCopyright(Vsix.Author)]
[assembly: AssemblyVersion(Vsix.Version)]
[assembly: AssemblyFileVersion(Vsix.Version)]

[assembly: InternalsVisibleTo("PackageReferenceVersionToAttributeExtensionTests")]