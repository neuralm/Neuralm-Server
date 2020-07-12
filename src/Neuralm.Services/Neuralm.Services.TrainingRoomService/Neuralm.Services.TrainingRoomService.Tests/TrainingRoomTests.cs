using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neuralm.Services.Common.Patterns;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Domain.FactoryArguments;

namespace Neuralm.Services.TrainingRoomService.Tests
{
    [TestClass]
    public class TrainingRoomTests
    {
        private User _user;
        private TrainingRoom _trainingRoom;
        private TrainingRoomSettings _trainingRoomSettings;
        private IFactory<Organism, OrganismFactoryArgument> _organismFactory;

        [TestClass]
        public class AddOrganismTests : TrainingRoomTests
        {
            [TestInitialize]
            public void Initialize()
            {
                _user = new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "Jan"
                };
                Guid trainingRoomId = Guid.NewGuid();
                _organismFactory = new OrganismFactory();
                _trainingRoomSettings = new TrainingRoomSettings(trainingRoomId, 0, 2, 1, 1, 1, 0.4, 3, 0.05, 0.03, 0.75, 0.001, 1, 0.8, 0.1, 0.5, 0.25, 0, 15);
                _trainingRoom = new TrainingRoom(trainingRoomId, _user, "CoolRoom", _trainingRoomSettings, _organismFactory);
            }

            [TestMethod]
            public void AddOrganismResultsInANewSpeciesTest()
            {
                Organism organism = new Organism(0, _trainingRoomSettings);
                Assert.IsFalse(_trainingRoom.Species.Any());
                _trainingRoom.AddOrganism(organism);
                Assert.IsTrue(_trainingRoom.Species.Any());
            }

            [TestMethod]
            public void AddOrganismCreatesOnly1SpeciesFor2OrganismsInGeneration0Test()
            {
                Organism organism1 = new Organism(0, _trainingRoomSettings);
                Organism organism2 = new Organism(0, _trainingRoomSettings);
                _trainingRoom.AddOrganism(organism1);
                _trainingRoom.AddOrganism(organism2);
                Assert.IsTrue(_trainingRoom.Species.Count == 1);
            }
        }
    }
}