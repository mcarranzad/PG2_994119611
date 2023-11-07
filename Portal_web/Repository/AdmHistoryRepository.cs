using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using sgc.ml.Dto.Reportes;
using sgc.ml.Models;
using sgc.ml.Repository.Interfaces;
using sgc.ml.Resources;
using sgc.ml.Util;

namespace sgc.ml.Repository;

public class AdmHistoryRepository : IAdmHistoryRepository
{
    private readonly DataContext dbContext;

    public AdmHistoryRepository(DataContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public List<AdmHistory> GetAll(string date_ini, string date_end)
    {
        try
        {
            string query =
                $"select * from adm_history where date_created  between '{date_ini} 00:00' and '{date_end} 23:59'";

            string finalQuery = SgcFunctions.QueryTransform(query.ToString(), "municipio");

            return dbContext
                .history
                .FromSqlRaw(finalQuery).ToList();
            
            
        }
        catch (Exception ex)
        {
            return new List<AdmHistory>();
        }
    }

    public List<MunicipiosConsultasDto> getMunicipiosQuePagan(string date_ini, string date_end)
    {
        try
        {
            string query = $"select municipio,\n     ifnull(((select count(*)\n              " +
                           $"from adm_history\n              where municipio = adm_cliente.municipio\n               " +
                           $" and prediccion = 'El cliente pagara su deuda'\n             " +
                           $"and date_created between '{date_ini} 00:00' and '{date_end} 23:59:59')), 0) verificacion_a,\n   " +
                           $"  ifnull(((select count(*)\n              from adm_history\n          " +
                           $"    where municipio = adm_cliente.municipio\n           " +
                           $"     and prediccion = 'El cliente no pagara su deuda'\n      " +
                           $"          and date_created between '{date_ini} 00:00' and '{date_end} 23:59:59')), 0) verificacion_b,\n " +
                           $"    ifnull(((select sum(deuda_b.saldoCastigoQuetzal)\n              from adm_history history_b\n " +
                           $"             inner join adm_cliente cliente_b\n              on cliente_b.dpi = history_b.dpi\n     " +
                           $"         inner join adm_deuda deuda_b\n                        on deuda_b.clienteId = cliente_b.clienteId\n  " +
                           $"            where history_b.municipio = adm_cliente.municipio\n             " +
                           $"   and history_b.prediccion = 'El cliente pagara su deuda'\n            " +
                           $"    and history_b.date_created between '{date_ini} 00:00' and '{date_end} 23:59:59')), 0) recuperacion_a,\n" +
                           $"          ifnull(((select sum(deuda_b.saldoCastigoQuetzal)\n              from adm_history history_b\n    " +
                           $"          inner join adm_cliente cliente_b\n              on cliente_b.dpi = history_b.dpi\n       " +
                           $"   inner join adm_deuda deuda_b\n                        on deuda_b.clienteId = cliente_b.clienteId\n   " +
                           $"   where history_b.municipio = adm_cliente.municipio\n             " +
                           $"   and history_b.prediccion = 'El cliente no pagara su deuda'\n       " +
                           $"   and history_b.date_created between '{date_ini} 00:00' and '{date_end} 23:59:59')), 0) recuperacion_b\n " +
                           $"   from adm_cliente\ngroup by municipio\norder by municipio asc";

            string finalQuery = SgcFunctions.QueryTransform(query.ToString(), "municipio");

            return dbContext
                .MunicipiosConsultas
                .FromSqlRaw(finalQuery).ToList();
        }
        catch (Exception ex)
        {
            return new List<MunicipiosConsultasDto>();
        }
    }


    public List<MunicipiosDto> getMunicipios()
    {
        try
        {
            StringBuilder query = new StringBuilder();
            query.Append("select municipio\n from adm_cliente \n group by municipio order by municipio asc ");
            return dbContext.municipios.FromSqlRaw(SgcFunctions.QueryTransform(query.ToString(), "municipio")).ToList();
        }
        catch (Exception ex)
        {
            return new List<MunicipiosDto>();
        }
    }


    /// <summary>
    /// Este metodo devuelve la informacion 
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    public List<DistribucionAcertividadDto> GetDistribucionAcertividad(int year, int month)
    {
        try
        {
            StringBuilder query = new StringBuilder();
            query.Append("select  prediccion, count(*) cantidad, (sum(precition) / count(*)) acertividad \n");
            query.Append("from adm_history \n");
            query.Append(string.Format(" where date_format(date_created, '%Y') = {0} \n ", year));

            if (month > 0)
                query.Append(string.Format("and date_format(date_created, '%m')={0} \n", month));

            query.Append("and prediccion not in ('Sin Resultados') \n");
            query.Append("group by  prediccion \n");

            var finalQuery = SgcFunctions.QueryTransform(query.ToString(), "acertividad");

            return dbContext.distribucionacertividad.FromSqlRaw(finalQuery).ToList();
        }
        catch (Exception ex)
        {
            return new List<DistribucionAcertividadDto>();
        }
    }


    /// <summary>
    /// Este metodo persiste los historicos que vienen de eventix
    /// </summary>
    /// <param name="model"></param>
    public void Persist(AdmHistory model)
    {
        model.history_id = 0;
        model.date_created = SgcFunctions.getNow();
        dbContext.Add(model);
        Save();
    }


    public void Save()
    {
        try
        {
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}