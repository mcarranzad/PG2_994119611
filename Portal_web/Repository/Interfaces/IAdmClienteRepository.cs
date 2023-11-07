using System.Xml.Linq;
using sgc.ml.Models;

namespace sgc.ml.Repository.Interfaces;

public interface IAdmClienteRepository
{

    IEnumerable<AdmCliente> getAll();

    AdmCliente? getOne(long dpi);

}
