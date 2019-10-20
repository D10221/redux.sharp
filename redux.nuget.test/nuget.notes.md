I Haven't tested it yet on Mac but it worked on Linux

## Publish

- Enable GPR 'feature' in your GitBHub settings
- Go to github and add a TOKEN with GPR read/write access
- Add source locally

        nuget source Add -Name "GitHub" \
          -Source "https://nuget.pkg.github.com/MY_ACCOUNT/index.json"

- Set api key

        nuget setApiKey $TOKEN \
            -Source "https://nuget.pkg.github.com/MY_ACCOUNT/index.json"

### Push package

        nuget push "my.lib.nupkg" -Source "GitHub"

### Install package

        nuget install my.lib -pre # '-pre' because of alpha, if alpha

<pre>
Note:  
'nuget install' downloads the nuget package  
It doesn't add it to the project.  
'dotnet' can't find it  
do it anyway so it gets cached  
the locally  downloaded package can be deleted  
</pre>

*Reference manually:*

Add 'new Source' to nuget.config.  

`<add key="GitHub" value="https://nuget.pkg.github.com/MY_ACCOUNT/index.json" />`

nuget config can be

- './nuget.config'
- '~/.nuget/NuGet/nuget.config'
- '%appdata%\NuGet\NuGet.Config'  

*Add package to project*:

        #  -v with version or version will be '*'
        #  -n "don't download", it can't handle authentication 
        dotnet add package my.lib \
            -s "https://nuget.pkg.github.com/MY_ACCOUNT/index.json" \
            -v 1.0.0-alpha \ 
            -n 

*Alternatively*:  
Edit project package reference  

`<PackageReference Include="my.lib" Version="1.0.0-alpha" />`

Finally:

    dotnet restore 
