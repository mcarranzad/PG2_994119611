# PORTAL - PG 2 

El proyecto denominado Portal PG2 es una implementacion de .NET 6 con soporte para C#/ASP.NET MVC CORE en el front end


## Background
Se han usado las siguientes tecnologias y patrones

- LINQ
- DbContext
- ASP.NET MVC
- JWT
- KESTREL

## Notes for development
Se recomienda trabajar con los siguientes directorios

-  app
    - application
        -  /util to add change CONSTANTS and important CONFIG
        -  /controller to add interactivity between routes and controllers
        -  /dto  to add classes as data transfer objects customizable
        -  /repository to add services(dao) bus that would be used by controllers
        -  /repository/interfaces to add interfaces for repositories
        -  /model to save principal entities 
    


## Usar la inyeccion de dependencias
 Para Registrar repositorios   util/EntityDbContext.cs se recomienda seguir los siguientes pasos en program.cs

```
/*Register models to use in dbSet */
public DbSet<INeuronalService> neuronals { get; set; }

```


To register repositories go  to Startup.cs and  register on services.AddTransient
```

/*Register Repositories and interfaces */
services.AddTransient<INeuronalServiceAdmGame, NeuronalService>();

```



## Currently this project supports

- NewTonJson 3.1.3
- FrameWorkCore.Design 5.0
- Extensions 5.0
- SeriLog.Sink.Console 4.0
- Serilog 2.1
- Swashbuckle 5.5

## BUILD
you can use buldAndRun.sh to  download/build/run changes from git repository 



