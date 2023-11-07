using Microsoft.EntityFrameworkCore;
using sgc.ml.Dto.Reportes;
using sgc.ml.Models;

namespace sgc.ml.Resources;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;


    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings
        if (!options.IsConfigured)
            options.UseMySQL(Configuration.GetConnectionString("Default"));
    }


    public DbSet<AdmCliente> clientes { get; set; }

    public DbSet<AdmHistory> history { get; set; }

    //reportes

    public DbSet<DistribucionAcertividadDto> distribucionacertividad { get; set; }

    public DbSet<MunicipiosDto> municipios { get; set; }

    public DbSet<MunicipiosConsultasDto> MunicipiosConsultas { get; set; }

    public DbSet<MunicipiosConsultasDto> MunicipiosConsultasb { get; set; }

    public DbSet<AdmUsuario> usuarios { get; set; }
    
    public DbSet<AdmRol> rols { get; set; }
    
    public DbSet<AdmPregunta> preguntas { get; set; }
}