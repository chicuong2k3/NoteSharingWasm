using Microsoft.AspNetCore.SignalR;

namespace SharingNote.Api.Hubs;

public class ViewHub : Hub
{
    private static int _viewCount = 0;
    public override async Task OnConnectedAsync()
    {
        _viewCount++;
        await Clients.All.SendAsync("viewCountUpdate", _viewCount);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _viewCount--;
        await Clients.All.SendAsync("viewCountUpdate", _viewCount);
        await base.OnDisconnectedAsync(exception);
    }
}
