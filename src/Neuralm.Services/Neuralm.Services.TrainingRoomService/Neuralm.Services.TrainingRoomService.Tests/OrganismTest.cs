using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neuralm.Services.Common.Patterns;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Domain.FactoryArguments;
using Neuralm.Services.TrainingRoomService.Tests.Evaluatables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuralm.Services.TrainingRoomService.Tests
{
    [TestClass]
    public class OrganismTests
    {
        private TrainingRoom _trainingRoom;
        private IFactory<Organism, OrganismFactoryArgument> _organismFactory;
        private User _fakeUser;
        private string _roomName;

        [TestClass]
        public class CrossOverTests : OrganismTests
        {
            [TestInitialize]
            public void Initialize()
            {
                _fakeUser = new User(); 
                Guid trainingRoomId = Guid.NewGuid();
                _roomName = "CoolRoom";
                _organismFactory = new OrganismFactory();
                _trainingRoom = new TrainingRoom(trainingRoomId, _fakeUser, _roomName, new TrainingRoomSettings(trainingRoomId, 0, 2, 1, 1, 1, 0.4, 3, 0.05, 0.03, 0.75, 0.001, 1, 0.8, 0.1, 0.5, 0.25, 0), _organismFactory);
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

                Organism child = organism1.Crossover(organism2, _trainingRoom.TrainingRoomSettings, _organismFactory);
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
                Guid trainingRoomId = Guid.NewGuid();
                _organismFactory = new OrganismFactory();
                _trainingRoom = new TrainingRoom(trainingRoomId, _fakeUser, "FakeRoom", new TrainingRoomSettings(trainingRoomId, 0, 2, 3, 1, 1, 0.4, 3, 0.05, 0.03, 0.75, 0.001, 1, 0.8, 0.1, 0.5, 0.25, 0), _organismFactory);
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
                _original.Score = 0;
                Organism clone = _original.Clone(_trainingRoom.TrainingRoomSettings);
                clone.Score = 100;
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
                Guid trainingRoomId = Guid.NewGuid();
                _roomName = "dfsd";
                _fakeUser = new User();
                _organismFactory = new OrganismFactory();
                _trainingRoom = new TrainingRoom(trainingRoomId, _fakeUser, _roomName, new TrainingRoomSettings(trainingRoomId, 0, 3, 1, 1, 1, 0.4, 3, 0.05, 0.03, 0.75, 0.001, 1, 0.8, 0.1, 0.5, 0.25, 0), _organismFactory);
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
    
        [TestClass]
        public class MutateTest : OrganismTests
        {
            [TestInitialize]
            public void Initialize()
            {
                _fakeUser = new User();
                Guid trainingRoomId = Guid.NewGuid();
                _roomName = "CoolRoom";
                _organismFactory = new OrganismFactory();
                //Create a training room with really high mutation settings
                TrainingRoomSettings trainingRoomSettings = new TrainingRoomSettings(trainingRoomId: trainingRoomId,
                                                                                     organismCount: 100,
                                                                                     inputCount: 2,
                                                                                     outputCount: 1,
                                                                                     c1: 1,
                                                                                     c2: 1,
                                                                                     c3: 0.4,
                                                                                     threshold: 3,
                                                                                     addConnectionChance: 1,
                                                                                     addNodeChance: 1,
                                                                                     crossOverChance: 0.75,
                                                                                     interSpeciesChance: 0.001,
                                                                                     mutationChance: 1,
                                                                                     mutateWeightChance: 0.8,
                                                                                     weightReassignChance: 0.1,
                                                                                     topAmountToSurvive: 0.5,
                                                                                     enableConnectionChance: 0.25,
                                                                                     seed: 0);

                _trainingRoom = new TrainingRoom(trainingRoomId, _fakeUser, _roomName, trainingRoomSettings, _organismFactory);

                for (int i = 0; i < trainingRoomSettings.OrganismCount; i++)
                {
                    Organism organism = new Organism(trainingRoomSettings, _trainingRoom.GetInnovationNumber) { IsLeased = true };
                    _trainingRoom.AddOrganism(organism);
                }
                _trainingRoom.IncreaseNodeIdTo(trainingRoomSettings.InputCount + trainingRoomSettings.OutputCount);
            }

            [TestMethod]
            public void Mutate_15_Generations()
            {
                //Run 15 generations
                for (int i = 0; i < 15; i++)
                {
                    _trainingRoom.Species.ForEach(species => species.Organisms.ForEach(o => { o.Score = 1; o.IsEvaluated = true; }));
                    _trainingRoom.EndGeneration(o => { });
                }
                
                Assert.AreEqual(15, (int)_trainingRoom.Generation);
            }
        }

        [TestClass]
        public class XorTest : OrganismTests
        {
            [TestInitialize]
            public void Initialize()
            {
                _fakeUser = new User();
                Guid trainingRoomId = Guid.NewGuid();
                _roomName = "CoolRoom";

                //Create a training room with really high mutation settings
                TrainingRoomSettings trainingRoomSettings = new TrainingRoomSettings(trainingRoomId: trainingRoomId,
                                                                                     organismCount: 25,
                                                                                     inputCount: 3,
                                                                                     outputCount: 1,
                                                                                     c1: 1,
                                                                                     c2: 1,
                                                                                     c3: 0.4,
                                                                                     threshold: 3,
                                                                                     addConnectionChance: 1,
                                                                                     addNodeChance: 1,
                                                                                     crossOverChance: 0.75,
                                                                                     interSpeciesChance: 0.001,
                                                                                     mutationChance: 1,
                                                                                     mutateWeightChance: 0.8,
                                                                                     weightReassignChance: 0.1,
                                                                                     topAmountToSurvive: 0.5,
                                                                                     enableConnectionChance: 0.25,
                                                                                     seed: 1);
                _organismFactory = new EvaluatableOrganismFactory();
                _trainingRoom = new TrainingRoom(trainingRoomId, _fakeUser, _roomName, trainingRoomSettings, _organismFactory);

                for (int i = 0; i < trainingRoomSettings.OrganismCount; i++)
                {
                    EvaluatableOrganism organism = new EvaluatableOrganism(trainingRoomSettings, _trainingRoom.GetInnovationNumber) { IsLeased = true };
                    _trainingRoom.AddOrganism(organism);
                }
                _trainingRoom.IncreaseNodeIdTo(trainingRoomSettings.InputCount + trainingRoomSettings.OutputCount);
            }

            [TestMethod]
            public void Xor()
            {
                Xor xor = new Xor();
                //Run 15 generations
                for (int i = 0; i < 15; i++)
                {
                    _trainingRoom.Species.ForEach(species => species.Organisms.ForEach(o =>
                    {
                        xor.Test((EvaluatableOrganism) o);
                        o.IsEvaluated = true;
                    }));
                    _trainingRoom.EndGeneration(o => { });
                }

                Assert.AreEqual(15, (int)_trainingRoom.Generation);
            }
        }
    }
}