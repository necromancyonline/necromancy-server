using System.Collections.Generic;

namespace Necromancy.Server.Model
{
    public class ClientLookup
    {
        private readonly List<NecClient> _clients;

        private readonly object _lock = new object();

        public ClientLookup()
        {
            _clients = new List<NecClient>();
        }

        public int GetCount()
        {
            lock (_lock)
            {
                return _clients.Count;
            }
        }

        /// <summary>
        ///     Returns all Clients.
        /// </summary>
        public List<NecClient> GetAll()
        {
            lock (_lock)
            {
                return new List<NecClient>(_clients);
            }
        }

        /// <summary>
        ///     Adds a Client.
        /// </summary>
        public void Add(NecClient client)
        {
            if (client == null) return;

            lock (_lock)
            {
                _clients.Add(client);
            }
        }

        /// <summary>
        ///     Removes the Client from all lists and lookup tables.
        /// </summary>
        public void Remove(NecClient client)
        {
            lock (_lock)
            {
                _clients.Remove(client);
            }
        }

        /// <summary>
        ///     Returns a Client by soul name if it exists.
        /// </summary>
        public NecClient GetBySoulName(string soulName)
        {
            List<NecClient> clients = GetAll();
            foreach (NecClient client in clients)
            {
                Soul soul = client.soul;
                if (soul != null && soul.name == soulName) return client;
            }

            return null;
        }

        /// <summary>
        ///     Returns a Client by characterId if it exists.
        /// </summary>
        public NecClient GetByCharacterId(int characterId)
        {
            List<NecClient> clients = GetAll();
            foreach (NecClient client in clients)
            {
                Character character = client.character;
                if (character != null && character.id == characterId) return client;
            }

            return null;
        }

        /// <summary>
        ///     Returns a Character by CharacterName if it exists or null.
        /// </summary>
        public Character GetCharacterByCharacterId(int characterId)
        {
            NecClient client = GetByCharacterId(characterId);
            if (client == null) return null;

            Character character = client.character;
            return character;
        }

        /// <summary>
        ///     Returns a Client by AccountId if it exists.
        /// </summary>
        public NecClient GetByAccountId(int accountId)
        {
            List<NecClient> clients = GetAll();
            foreach (NecClient client in clients)
            {
                Account account = client.account;
                if (account != null && account.id == accountId) return client;
            }

            return null;
        }

        /// <summary>
        ///     Returns a Client by instanceId if it exists.
        /// </summary>
        public NecClient GetByCharacterInstanceId(uint instanceId)
        {
            List<NecClient> clients = GetAll();
            foreach (NecClient client in clients)
            {
                Character character = client.character;
                if (character != null && character.instanceId == instanceId) return client;
            }

            return null;
        }
    }
}
