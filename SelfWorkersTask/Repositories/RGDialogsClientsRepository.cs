using SelfWorkersTask.Models;

namespace SelfWorkersTask.Repositories;

public class RGDialogsClientsRepository : IRGDialogsClientsRepository
{
    public List<RGDialogsClients> GetRgDialogsClientsList()
    {
        return new RGDialogsClients().Init();
    }
}