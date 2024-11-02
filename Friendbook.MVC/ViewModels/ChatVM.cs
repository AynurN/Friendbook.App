using Friendbook.Core.Entities;

namespace Friendbook.MVC.ViewModels
{
    public record ChatVM(List<DirectMessage> Messages, string ReceiverId);
    
}
