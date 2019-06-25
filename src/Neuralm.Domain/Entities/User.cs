using System;
using System.Collections.Generic;

namespace Neuralm.Domain.Entities
{
    public class User
    {
        private readonly List<Room> _ownedRooms;
        private readonly List<Room> _authorizedRooms;
        private readonly List<Session> _currentSessions;

        public Guid Id { get; }
        public string Username { get; }
        public IReadOnlyList<Room> OwnedRooms => _ownedRooms;
        public IReadOnlyList<Room> AuthorizedRooms => _authorizedRooms;
        public IReadOnlyList<Session> CurrentSessions => _currentSessions;

        public User(string username)
        {
            Id = Guid.NewGuid();
            Username = username;
            _ownedRooms = new List<Room>();
            _authorizedRooms = new List<Room>();
            _currentSessions = new List<Session>();
        }

        public Room CreateRoom(string name)
        {
            Room room = new Room(this, name);
            _ownedRooms.Add(room);
            return room;
        }

        public Session CreateSessionForRoom(Room room)
        {
            Session session = new Session(room, this);
            _currentSessions.Add(session);
            return session;
        }
    }
}
