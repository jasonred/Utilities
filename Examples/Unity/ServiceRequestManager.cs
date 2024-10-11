using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Utilities;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Managers
{
    public class ServiceRequestManager
    {
        private TaskQueueService<Lobby> createLobbyQueue = new(6000, 2);
        private TaskQueueService<Lobby> joinLobbyByIdQueue = new(1000, 2);
        private TaskQueueService<Lobby> reconnectToLobbyQueue = new(1000, 1);
        private TaskQueueService<Lobby> getLobbyQueue = new(1000, 1);
        private TaskQueueService<List<string>> getJoinedLobbiesQueue = new(1000, 1);
        private TaskQueueService<QueryResponse> queryLobbiesQueue = new(1000, 1);
        private TaskQueueService deleteLobbyQueue = new(1000, 2);
        private TaskQueueService sendHeartbeatPingQueue = new(1000, 1);
        private TaskQueueService<Lobby> updatePlayerQueue = new(5000, 5);
        private TaskQueueService removePlayerQueue = new(1000, 5);

        public async Task<Lobby> CreateLobbyAsync(string lobbyName, int maxPlayers, CreateLobbyOptions options = null) => 
            await createLobbyQueue.EnqueueRequest(() => LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options));

        public async Task<Lobby> JoinLobbyByIdAsync(string lobbyId, JoinLobbyByIdOptions options = null) =>
            await joinLobbyByIdQueue.EnqueueRequest(() => LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, options));

        public async Task<Lobby> ReconnectToLobbyAsync(string lobbyId) => 
            await reconnectToLobbyQueue.EnqueueRequest(() => LobbyService.Instance.ReconnectToLobbyAsync(lobbyId));

        public async Task<Lobby> GetLobbyAsync(string lobbyId) => 
            await getLobbyQueue.EnqueueRequest(() => LobbyService.Instance.GetLobbyAsync(lobbyId));

        public async Task<List<string>> GetJoinedLobbiesAsync() => 
            await getJoinedLobbiesQueue.EnqueueRequest(() => LobbyService.Instance.GetJoinedLobbiesAsync());

        public async Task<QueryResponse> QueryLobbiesAsync(QueryLobbiesOptions options = null) => 
            await queryLobbiesQueue.EnqueueRequest(() => LobbyService.Instance.QueryLobbiesAsync(options));

        public async Task<Lobby> UpdatePlayerAsync(string lobbyId, string playerId, UpdatePlayerOptions options) => 
            await updatePlayerQueue.EnqueueRequest(() => LobbyService.Instance.UpdatePlayerAsync(lobbyId, playerId, options));

        public async Task DeleteLobbyAsync(string lobbyId) => 
            await deleteLobbyQueue.EnqueueRequest(() => LobbyService.Instance.DeleteLobbyAsync(lobbyId));

        public async Task SendHeartbeatPingAsync(string lobbyId) => 
            await sendHeartbeatPingQueue.EnqueueRequest(() => LobbyService.Instance.SendHeartbeatPingAsync(lobbyId));

        public async Task RemovePlayerAsync(string lobbyId, string playerId) => 
            await removePlayerQueue.EnqueueRequest(() => LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId));
    }
}
