using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neuralm.Domain.Entities;
using Neuralm.Domain.Entities.NEAT;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuralm.Tests.NEAT
{
    [TestClass]
    public class BrainTests
    {
        private TrainingRoom _trainingRoom;
        private User _fakeUser;

        [TestInitialize]
        public void Initialize()
        {
            _fakeUser = new User();
            _trainingRoom = new TrainingRoom(_fakeUser, "FakeRoom", new TrainingRoomSettings(0, 2, 1, 1, 1, 0.4, 3, 0.05, 0.03, 0.75, 0.001, 1, 0.8, 0.1, 0.5, 0.25, 0));
        }

        [TestMethod]
        public void CrossOverTest()
        {
            Brain expectedBrain = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>() {
                new ConnectionGene(0, 3, 1, 1),
                new ConnectionGene(1, 3, 1, 2, false),
                new ConnectionGene(2, 3, 1, 3),
                new ConnectionGene(1, 4, -1, 4),
                new ConnectionGene(4, 3, 1, 5, false),
                new ConnectionGene(4, 5, 1, 6),
                new ConnectionGene(5, 3, 1, 7),
                new ConnectionGene(0, 4, -1, 8),
                new ConnectionGene(2, 4, 1, 9),
                new ConnectionGene(0, 5, 1, 10)
            });
            List<ConnectionGene> expectedGenes = expectedBrain.Genes.Values.OrderBy(x => x.InnovationNumber).ToList();

            Brain brain1 = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>()
            {
                new ConnectionGene(0, 3, -1, 1),
                new ConnectionGene(1, 3, -1, 2, false),
                new ConnectionGene(2, 3, -1, 3),
                new ConnectionGene(1, 4, -1, 4),
                new ConnectionGene(4, 3, -1, 5),
                new ConnectionGene(0, 4, -1, 8)
            });

            Brain brain2 = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>()
            {
                new ConnectionGene(0, 3, 1, 1),
                new ConnectionGene(1, 3, 1, 2, false),
                new ConnectionGene(2, 3, 1, 3),
                new ConnectionGene(1, 4, 1, 4),
                new ConnectionGene(4, 3, 1, 5, false),
                new ConnectionGene(4, 5, 1, 6),
                new ConnectionGene(5, 3, 1, 7),
                new ConnectionGene(2, 4, 1, 9),
                new ConnectionGene(0, 5, 1, 10)
            });

            Brain child = brain1.Crossover(brain2);

            List<ConnectionGene> childGenes = child.Genes.Values.OrderBy(a => a.InnovationNumber).ToList();

            CollectionAssert.AreEqual(childGenes, expectedGenes);
        }

        [TestClass]
        public class Clone
        {
            private User _fakeUser;
            private TrainingRoom _trainingRoom;
            private Brain _original;

            [TestInitialize]
            public void Initialize()
            {
                _fakeUser = new User();
                _trainingRoom = new TrainingRoom(_fakeUser, "FakeRoom", new TrainingRoomSettings(0, 2, 1, 1, 1, 0.4, 3, 0.05, 0.03, 0.75, 0.001, 1, 0.8, 0.1, 0.5, 0.25, 0));
                _original = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>()
                {
                    new ConnectionGene(0, 3, 1, 1),
                    new ConnectionGene(1, 3, 1, 2, false),
                    new ConnectionGene(2, 3, 1, 3),
                    new ConnectionGene(1, 4, 1, 4),
                    new ConnectionGene(4, 3, 1, 5),
                    new ConnectionGene(0, 4, 1, 6)
                });
            }

            [TestMethod]
            public void CloneTest()
            {
                Brain clone = _original.Clone();

                Assert.AreEqual(clone, _original);
            }

            [TestMethod]
            public void CloneDoesNotAffectOriginalScoreTest()
            {
                _original.Score = 0;
                Brain clone = _original.Clone();
                clone.Score = 100;

                Assert.AreEqual(_original.Score, 0);
            }

            [TestMethod]
            public void CloneDoesNotAffectOriginalGeneTest()
            {
                List<ConnectionGene> originalGenes = new List<ConnectionGene>(_original.Genes.Values);
                foreach (ConnectionGene gene in originalGenes)
                {
                    _trainingRoom.GetInnovationNumber(gene.InId, gene.OutId);
                    _trainingRoom.IncreaseNodeIdTo(Math.Max(gene.InId, gene.OutId)+1);
                }

                Brain clone = _original.Clone();
                for (int i = 0; i < 100; i++)
                {
                    clone.Mutate();
                }

                CollectionAssert.AreEqual(_original.Genes.Values.ToList(), originalGenes);
            }
        }


        [TestClass]
        public class EqualsTest
        {
            private TrainingRoom _trainingRoom;
            private User _fakeUser;

            [TestInitialize]
            public void Initialize()
            {
                _fakeUser = new User();
                _trainingRoom = new TrainingRoom(_fakeUser, "FakeRoom", new TrainingRoomSettings(0, 2, 1, 1, 1, 0.4, 3, 0.05, 0.03, 0.75, 0.001, 1, 0.8, 0.1, 0.5, 0.25, 0));
            }

            [TestMethod]
            public void AreEqualTest()
            {
                Brain brain1 = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>() {
                    new ConnectionGene(0, 3, 1, 1),
                    new ConnectionGene(1, 3, 1, 2, false),
                    new ConnectionGene(2, 3, 1, 3),
                    new ConnectionGene(1, 4, 1, 4),
                    new ConnectionGene(4, 3, 1, 5),
                    new ConnectionGene(0, 4, 1, 8)
                });

                Brain brain2 = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>() {
                    new ConnectionGene(0, 3, 1, 1),
                    new ConnectionGene(1, 3, 1, 2, false),
                    new ConnectionGene(2, 3, 1, 3),
                    new ConnectionGene(1, 4, 1, 4),
                    new ConnectionGene(4, 3, 1, 5),
                    new ConnectionGene(0, 4, 1, 8)
                });

                Assert.IsTrue(brain1.Equals(brain2));
            }

            [TestMethod]
            public void MissingGeneTest()
            {
                Brain brain1 = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>() {
                    new ConnectionGene(0, 3, 1, 1),
                    new ConnectionGene(1, 3, 1, 2, false),
                    new ConnectionGene(2, 3, 1, 3),
                    new ConnectionGene(1, 4, 1, 4),
                    new ConnectionGene(4, 3, 1, 5),
                    new ConnectionGene(0, 4, 1, 8)
                });

                Brain brain2 = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>() {
                    new ConnectionGene(0, 3, 1, 1),
                    new ConnectionGene(1, 3, 1, 2, false),
                    new ConnectionGene(2, 3, 1, 3),
                    new ConnectionGene(1, 4, 1, 4),
                    new ConnectionGene(4, 3, 1, 5)
                });

                Assert.IsFalse(brain1.Equals(brain2));
            }

            [TestMethod]
            public void ExtraGeneTest()
            {
                Brain brain1 = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>() {
                    new ConnectionGene(0, 3, 1, 1),
                    new ConnectionGene(1, 3, 1, 2, false),
                    new ConnectionGene(2, 3, 1, 3),
                    new ConnectionGene(1, 4, 1, 4),
                    new ConnectionGene(4, 3, 1, 5),
                    new ConnectionGene(0, 4, 1, 8)
                });

                Brain brain2 = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>() {
                    new ConnectionGene(0, 3, 1, 1),
                    new ConnectionGene(1, 3, 1, 2, false),
                    new ConnectionGene(2, 3, 1, 3),
                    new ConnectionGene(1, 4, 1, 4),
                    new ConnectionGene(4, 3, 1, 5),
                    new ConnectionGene(0, 4, 1, 8),
                    new ConnectionGene(0, 2, 1, 9)
                });

                Assert.IsFalse(brain1.Equals(brain2));
            }

            [TestMethod]
            public void DifferentInputSizeTest()
            {
                Brain brain1 = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>() {
                    new ConnectionGene(0, 3, 1, 1),
                    new ConnectionGene(1, 3, 1, 2, false),
                    new ConnectionGene(2, 3, 1, 3),
                    new ConnectionGene(1, 4, 1, 4),
                    new ConnectionGene(4, 3, 1, 5),
                    new ConnectionGene(0, 4, 1, 8)
                });

                Brain brain2 = new Brain(4, 1, _trainingRoom, new List<ConnectionGene>() {
                    new ConnectionGene(0, 3, 1, 1),
                    new ConnectionGene(1, 3, 1, 2, false),
                    new ConnectionGene(2, 3, 1, 3),
                    new ConnectionGene(1, 4, 1, 4),
                    new ConnectionGene(4, 3, 1, 5),
                    new ConnectionGene(0, 4, 1, 8)
                });

                Assert.IsFalse(brain1.Equals(brain2));
            }

            [TestMethod]
            public void DifferentOutputSizeTest()
            {
                Brain brain1 = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>() {
                    new ConnectionGene(0, 3, 1, 1),
                    new ConnectionGene(1, 3, 1, 2, false),
                    new ConnectionGene(2, 3, 1, 3),
                    new ConnectionGene(1, 4, 1, 4),
                    new ConnectionGene(4, 3, 1, 5),
                    new ConnectionGene(0, 4, 1, 8)
                });

                Brain brain2 = new Brain(3, 2, _trainingRoom, new List<ConnectionGene>() {
                    new ConnectionGene(0, 3, 1, 1),
                    new ConnectionGene(1, 3, 1, 2, false),
                    new ConnectionGene(2, 3, 1, 3),
                    new ConnectionGene(1, 4, 1, 4),
                    new ConnectionGene(4, 3, 1, 5),
                    new ConnectionGene(0, 4, 1, 8)
                });

                Assert.IsFalse(brain1.Equals(brain2));
            }

            [TestMethod]
            [DataRow(5)]
            [DataRow(null)]
            [DataRow(default)]
            [DataRow("brain")]
            [DataRow(new int[] { 1, 2, 3 })]
            public void DifferentTypeTest(object other)
            {
                Brain brain1 = new Brain(3, 1, _trainingRoom, new List<ConnectionGene>() {
                    new ConnectionGene(0, 3, 1, 1),
                    new ConnectionGene(1, 3, 1, 2, false),
                    new ConnectionGene(2, 3, 1, 3),
                    new ConnectionGene(1, 4, 1, 4),
                    new ConnectionGene(4, 3, 1, 5),
                    new ConnectionGene(0, 4, 1, 8)
                });

                Assert.IsFalse(brain1.Equals(other));
            }
        }
    }
}
