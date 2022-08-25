using Microsoft.AspNetCore.Mvc;
using SelfWorkersTask.Models;
using SelfWorkersTask.Repositories;

namespace SelfWorkersTask.Controllers;

[ApiController]
[Route("[controller]")]
public class RGDialogsController : ControllerBase
{
    private IRGDialogsClientsRepository _rgDialogsClientsRepository;
    
    public RGDialogsController(IRGDialogsClientsRepository rgDialogsClientsRepository)
    {
        _rgDialogsClientsRepository = rgDialogsClientsRepository ?? throw new ArgumentNullException(nameof(rgDialogsClientsRepository));
    }

    [HttpPost]
    public Guid SearchRGDialog([FromBody] Guid[] guids)
    {
        var guidsSet = new HashSet<Guid>(guids);
        var guidsSetCount = guidsSet.Count;
        var rbDialogsClients = _rgDialogsClientsRepository.GetRgDialogsClientsList();
        // Решение с использоваванием LINQ
        var rbdialog = rbDialogsClients
            .GroupBy(clients => clients.IDRGDialog)
            .FirstOrDefault(grouping => grouping.IntersectBy(guids, clients => clients.IDClient).Count() == guids.Length, null);
        
        // Решение без использовавания LINQ
        // TIME - O(N)
        // RAM - O(N)
        var counter = new Dictionary<Guid, int>(rbDialogsClients.Count);
        // O(N)
        foreach (var client in rbDialogsClients)
        {
            // O(1)
            if (guidsSet.Contains(client.IDClient))
            {
                // O(1)
                if (!counter.ContainsKey(client.IDRGDialog))
                {
                    // O(1) за счет создания словаря размера rbDialogsClients.Count
                    counter.Add(client.IDRGDialog, 1);
                }
                else
                {
                    // O(1)
                    counter[client.IDRGDialog] += 1;
                }
                // O(1)
                if (counter[client.IDRGDialog] == guidsSetCount)
                    return client.IDRGDialog;
            }
        }
        // O(N)
        return counter.FirstOrDefault(pair => pair.Value == guidsSet.Count).Key;
    }
}