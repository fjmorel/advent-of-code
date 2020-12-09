# Advent of Code projects


## How to run projects

It's possible to run simple C# files without a project file and I may write up a script to easily run specific days. In the meantime, this will work per project. Unfortunately, it looks like Omnisharp stops working if you delete the csproj :(

Install dotnet-script

```
dotnet tool install -g dotnet-script
```

Then go into whatever folder you want to run and do

```
dotnet script Program.cs
```