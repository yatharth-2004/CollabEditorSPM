using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CollabTextEditor.Data;
using CollabTextEditor.Models;

namespace CollabTextEditor.Hubs
{
    public class CollabHub : Hub
    {
        private readonly AppDbContext _context;

        public CollabHub(AppDbContext context)
        {
            _context = context;
        }

        // Called by clients to join a specific group.
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            
            // Optionally, send the current document content to the joining client.
            var document = await _context.Documents.FirstOrDefaultAsync();
            if (document != null)
            {
                await Clients.Caller.SendAsync("ReceiveText", document.Content);
            }
        }

        // Called by clients when the text changes.
        public async Task UpdateText(string groupName, string newText)
        {
            // Update the document content in the database.
            var document = await _context.Documents.FirstOrDefaultAsync();
            if (document != null)
            {
                document.Content = newText;
                _context.Documents.Update(document);
                await _context.SaveChangesAsync();
            }
            
            // Broadcast the new text to everyone in the group except the sender.
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveText", newText);
        }
    }
}
