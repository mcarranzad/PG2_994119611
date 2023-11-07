
using sgc.ml.Dto.Reportes;
using sgc.ml.Models;

namespace sgc.ml.Repository.Interfaces;

public interface IAdmHistoryRepository
{
    
    List<AdmHistory> GetAll(string date_ini, string date_end);

    List<MunicipiosConsultasDto> getMunicipiosQuePagan(string date_ini, string date_end);



    List<MunicipiosDto> getMunicipios();

    List<DistribucionAcertividadDto> GetDistribucionAcertividad(int year, int month);
    
    void Persist(AdmHistory model);

    void Save();

}