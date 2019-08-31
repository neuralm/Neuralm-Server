using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neuralm.Domain.Entities;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Tests.NEAT
{
    [TestClass]
    public class TrainingRoomTests
    {
        private User _user;
        private TrainingRoom _trainingRoom;
        private TrainingRoomSettings _trainingRoomSettings;

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
                _trainingRoomSettings = new TrainingRoomSettings(0, 2, 1, 1, 1, 0.4, 3, 0.05, 0.03, 0.75, 0.001, 1, 0.8, 0.1, 0.5, 0.25, 0);
                _trainingRoom = new TrainingRoom(_user, "CoolRoom", _trainingRoomSettings);
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
