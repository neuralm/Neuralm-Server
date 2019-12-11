using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neuralm.Services.TrainingRoomService.Domain;

namespace Neuralm.Services.TrainingRoomService.Tests
{
    [TestClass]
    public class OrganismTests
    {
        private TrainingRoom _trainingRoom;
        private User _fakeUser;
        private string _roomName;

        [TestClass]
        public class CrossOverTests : OrganismTests
        {
            [TestInitialize]
            public void Initialize()
            {
                _fakeUser = new User();
                _roomName = "CoolRoom";
                _trainingRoom = new TrainingRoom(_fakeUser, _roomName, new TrainingRoomSettings(0, 2, 1, 1, 1, 0.4, 3, 0.05, 0.03, 0.75, 0.001, 1, 0.8, 0.1, 0.5, 0.25, 0));
            }

            [TestMethod]
            public void CrossOverTest()
            {
                Guid expectedOrganismId = Guid.NewGuid();
                Organism expectedOrganism = new Organism(expectedOrganismId, _trainingRoom.TrainingRoomSettings, 0, new List<ConnectionGene>()
                {
                    new ConnectionGene(expectedOrganismId, 1, 0, 3, -1),
                    new ConnectionGene(expectedOrganismId, 2, 1, 3, 1, false),
                    new ConnectionGene(expectedOrganismId, 3, 2, 3, 1),
                    new ConnectionGene(expectedOrganismId, 4, 1, 4, -1),
                    new ConnectionGene(expectedOrganismId, 5, 4, 3, 1, false),
                    new ConnectionGene(expectedOrganismId, 6, 4, 5, 1),
                    new ConnectionGene(expectedOrganismId, 7, 5, 3, 1),
                    new ConnectionGene(expectedOrganismId, 8, 0, 4, -1),
                    new ConnectionGene(expectedOrganismId, 9, 2, 4, 1),
                    new ConnectionGene(expectedOrganismId, 10, 0, 5, 1)
                });
                List<ConnectionGene> expectedGenes = expectedOrganism.ConnectionGenes.OrderBy(x => x.InnovationNumber).ToList();

                Guid id1 = Guid.NewGuid();
                Organism organism1 = new Organism(id1, _trainingRoom.TrainingRoomSettings, 0, new List<ConnectionGene>()
                {
                    new ConnectionGene(id1, 1, 0, 3, -1),
                    new ConnectionGene(id1, 2, 1, 3, -1, false),
                    new ConnectionGene(id1, 3, 2, 3, -1),
                    new ConnectionGene(id1, 4, 1, 4, -1),
                    new ConnectionGene(id1, 5, 4, 3, -1),
                    new ConnectionGene(id1, 8, 0, 4, -1)
                });

                Guid id2 = Guid.NewGuid();
                Organism organism2 = new Organism(id2, _trainingRoom.TrainingRoomSettings, 0, new List<ConnectionGene>()
                {
                    new ConnectionGene(id2, 1, 0, 3, 1),
                    new ConnectionGene(id2, 2, 1, 3, 1, false),
                    new ConnectionGene(id2, 3, 2, 3, 1),
                    new ConnectionGene(id2, 4, 1, 4, 1),
                    new ConnectionGene(id2, 5, 4, 3, 1, false),
                    new ConnectionGene(id2, 6, 4, 5, 1),
                    new ConnectionGene(id2, 7, 5, 3, 1),
                    new ConnectionGene(id2, 9, 2, 4, 1),
                    new ConnectionGene(id2, 10, 0, 5, 1)
                });

                Organism child = organism1.Crossover(organism2, _trainingRoom.TrainingRoomSettings);
                List<ConnectionGene> childGenes = child.ConnectionGenes.OrderBy(a => a.InnovationNumber).ToList();
                CollectionAssert.AreEqual(childGenes, expectedGenes);
            }
        }

        [TestClass]
        public class CloneTests : OrganismTests
        {
            private Organism _original;

            [TestInitialize]
            public void Initialize()
            {
                _fakeUser = new User();
                _trainingRoom = new TrainingRoom(_fakeUser, "FakeRoom", new TrainingRoomSettings(0, 2, 3, 1, 1, 0.4, 3, 0.05, 0.03, 0.75, 0.001, 1, 0.8, 0.1, 0.5, 0.25, 0));
                Guid id = Guid.NewGuid();
                _original = new Organism(id, _trainingRoom.TrainingRoomSettings, 0, new List<ConnectionGene>()
                {
                    new ConnectionGene(id, 1, 0, 3, 1),
                    new ConnectionGene(id, 2, 1, 3, 1, false),
                    new ConnectionGene(id, 3, 2, 3, 1),
                    new ConnectionGene(id, 4, 1, 4, 1),
                    new ConnectionGene(id, 5, 4, 3, 1),
                    new ConnectionGene(id, 6, 0, 4, 1)
                });
            }

            [TestMethod]
            public void CloneTest()
            {
                Organism clone = _original.Clone(_trainingRoom.TrainingRoomSettings);

                Assert.IsTrue(clone.Equals(_original));
            }

            [TestMethod]
            public void CloneDoesNotAffectOriginalScoreTest()
            {
                _trainingRoom.PostScore(_original, 0);
                Organism clone = _original.Clone(_trainingRoom.TrainingRoomSettings);
                _trainingRoom.PostScore(clone, 100);
                Assert.AreEqual(0, _original.Score);
            }

            [TestMethod]
            public void CloneDoesNotAffectOriginalGeneTest()
            {
                List<ConnectionGene> originalGenes = new List<ConnectionGene>(_original.ConnectionGenes);
                foreach (ConnectionGene gene in originalGenes)
                {
                    _trainingRoom.GetInnovationNumber(gene.InNodeIdentifier, gene.OutNodeIdentifier);
                    _trainingRoom.IncreaseNodeIdTo(Math.Max(gene.InNodeIdentifier, gene.OutNodeIdentifier) + 1);
                }

                Organism clone = _original.Clone(_trainingRoom.TrainingRoomSettings);
                for (int i = 0; i < 100; i++)
                {
                    clone.Mutate(_trainingRoom.TrainingRoomSettings, _trainingRoom.GetAndIncreaseNodeId, _trainingRoom.GetInnovationNumber);
                }

                CollectionAssert.AreEqual(_original.ConnectionGenes.ToList(), originalGenes);
            }
        }

        [TestClass]
        public class EqualsTests : OrganismTests
        {
            [TestInitialize]
            public void Initialize()
            {
                _roomName = "dfsd";
                _fakeUser = new User();
                _trainingRoom = new TrainingRoom(_fakeUser, _roomName, new TrainingRoomSettings(0, 3, 1, 1, 1, 0.4, 3, 0.05, 0.03, 0.75, 0.001, 1, 0.8, 0.1, 0.5, 0.25, 0));
            }

            [TestMethod]
            public void AreEqualTest()
            {
                Guid id1 = Guid.NewGuid();
                Organism organism1 = new Organism(id1, _trainingRoom.TrainingRoomSettings, 0, new List<ConnectionGene>() {
                        new ConnectionGene(id1, 1, 0, 3, 1),
                        new ConnectionGene(id1, 2, 1, 3, 1, false),
                        new ConnectionGene(id1, 3, 2, 3, 1),
                        new ConnectionGene(id1, 4, 1, 4, 1),
                        new ConnectionGene(id1, 5, 4, 3, 1),
                        new ConnectionGene(id1, 8, 0, 4, 1)
                    });
                Guid id2 = Guid.NewGuid();
                Organism organism2 = new Organism(id2, _trainingRoom.TrainingRoomSettings, 0, new List<ConnectionGene>() {
                        new ConnectionGene(id2, 1, 0, 3, 1),
                        new ConnectionGene(id2, 2, 1, 3, 1, false),
                        new ConnectionGene(id2, 3, 2, 3, 1),
                        new ConnectionGene(id2, 4, 1, 4, 1),
                        new ConnectionGene(id2, 5, 4, 3, 1),
                        new ConnectionGene(id2, 8, 0, 4, 1)
                    });

                Assert.IsTrue(organism1.Equals(organism2));
            }

            [TestMethod]
            public void MissingGeneTest()
            {
                Guid id1 = Guid.NewGuid();
                Organism organism1 = new Organism(id1, _trainingRoom.TrainingRoomSettings, 0, new List<ConnectionGene>() {
                        new ConnectionGene(id1, 1, 0, 3, 1),
                        new ConnectionGene(id1, 2, 1, 3, 1, false),
                        new ConnectionGene(id1, 3, 2, 3, 1),
                        new ConnectionGene(id1, 4, 1, 4, 1),
                        new ConnectionGene(id1, 5, 4, 3, 1),
                        new ConnectionGene(id1, 8, 0, 4, 1)
                    });
                Guid id2 = Guid.NewGuid();
                Organism organism2 = new Organism(id2, _trainingRoom.TrainingRoomSettings, 0, new List<ConnectionGene>() {
                        new ConnectionGene(id2, 1, 0, 3, 1),
                        new ConnectionGene(id2, 2, 1, 3, 1, false),
                        new ConnectionGene(id2, 3, 2, 3, 1),
                        new ConnectionGene(id2, 4, 1, 4, 1),
                        new ConnectionGene(id2, 5, 4, 3, 1)
                    });

                Assert.IsFalse(organism1.Equals(organism2));
            }

            [TestMethod]
            public void ExtraGeneTest()
            {
                Guid id1 = Guid.NewGuid();
                Organism organism1 = new Organism(id1, _trainingRoom.TrainingRoomSettings, 0, new List<ConnectionGene>() {
                        new ConnectionGene(id1, 1, 0, 3, 1),
                        new ConnectionGene(id1, 2, 1, 3, 1, false),
                        new ConnectionGene(id1, 3, 2, 3, 1),
                        new ConnectionGene(id1, 4, 1, 4, 1),
                        new ConnectionGene(id1, 5, 4, 3, 1),
                        new ConnectionGene(id1, 8, 0, 4, 1)
                    });
                Guid id2 = Guid.NewGuid();
                Organism organism2 = new Organism(id2, _trainingRoom.TrainingRoomSettings, 0, new List<ConnectionGene>() {
                        new ConnectionGene(id2, 1, 0, 3, 1),
                        new ConnectionGene(id2, 2, 1, 3, 1, false),
                        new ConnectionGene(id2, 3, 2, 3, 1),
                        new ConnectionGene(id2, 4, 1, 4, 1),
                        new ConnectionGene(id2, 5, 4, 3, 1),
                        new ConnectionGene(id2, 8, 0, 4, 1),
                        new ConnectionGene(id2, 9, 0, 2, 1)
                    });

                Assert.IsFalse(organism1.Equals(organism2));
            }

            [TestMethod]
            [DataRow(5)]
            [DataRow(null)]
            [DataRow(default)]
            [DataRow("brain")]
            [DataRow(new[] { 1, 2, 3 })]
            public void DifferentTypeTest(object other)
            {
                Guid id = Guid.NewGuid();
                Organism organism1 = new Organism(id, _trainingRoom.TrainingRoomSettings, 0, new List<ConnectionGene>() {
                        new ConnectionGene(id, 1, 0, 3, 1),
                        new ConnectionGene(id, 2, 1, 3, 1, false),
                        new ConnectionGene(id, 3, 2, 3, 1),
                        new ConnectionGene(id, 4, 1, 4, 1),
                        new ConnectionGene(id, 5, 4, 3, 1),
                        new ConnectionGene(id, 8, 0, 4, 1)
                    });

                Assert.IsFalse(organism1.Equals(other));
            }
        }
    }
}