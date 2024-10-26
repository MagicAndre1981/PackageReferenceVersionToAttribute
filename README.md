# PackageReference Version to attribute

This Visual Studio extension converts PackageReferences Version child element to attribute.

It works with C# csproj Visual Studio project files.

[Download it from the Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=RamiAbughazaleh.PackageReferenceVersionToAttributeExtension)

## Getting started

1. Download and install the extension.  
2. Right-click on a project and select `Convert PackageReference Version elements to attributes...`.  

![Preview](Preview.png)

3. Wait until the process finishes.  
  Check the status bar or the `PackageReferences Version to Attribute Extension` pane in the `Output Window` for details.


## Technical Details

This extension will first create a backup of the project file.  
For example, `MyProject.csproj` will be copied to `MyProject.csproj.bak`.  

If the project file is source controlled, it will be checked out for modification.  

The `Version` child elements of `PackageReference` will be converted to a `Version` attribute.  
For example, the following elements in the `csproj` project file:
```
  <PackageReference Include="Newtonsoft.Json">
    <Version>13.0.3</Version>
  </PackageReference>
```

will be converted to this:
```
  <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```


## Troubleshooting

Check the `PackageReferences Version to Attribute Extension` pane in the `Output Window` for detailed logs.


## Rate and Review

Has this extension helped you at all?

If so, please rate and share it.

Thank you! :)
