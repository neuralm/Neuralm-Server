using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neuralm.Services.TrainingRoomService.Domain;

namespace Neuralm.Services.TrainingRoomService.Tests
{
   [TestClass]
    public class ConnectionGeneTests
    {
        private ConnectionGene _original;
        private Guid _id;

        [TestClass]
        public class CloneTests : ConnectionGeneTests
        {
            [TestInitialize]
            public void Initialize()
            {
                _id = Guid.NewGuid();
                _original = new ConnectionGene(_id, 1, 0, 2, 0.5, true);
            }

            [TestMethod]
            public void CloneTest()
            {
                ConnectionGene clone = _original.Clone(_id);
                Assert.AreEqual(_original, clone);
            }

            [TestMethod]
            public void CloneDoesNotAffectOriginalTest()
            {
                double originalWeight = _original.Weight;
                ConnectionGene clone = _original.Clone(_id);
                clone.Weight = 9001;
                Assert.AreEqual(_original.Weight, originalWeight);
            }
        }

        [TestClass]
        public class EqualsTests : ConnectionGeneTests
        {
            [TestMethod]
            public void AreEqualTest()
            {
                ConnectionGene gene1 = new ConnectionGene(Guid.NewGuid(), 1, 0, 2, 1, true);
                ConnectionGene gene2 = new ConnectionGene(Guid.NewGuid(), 1, 0, 2, 1, true);
                Assert.IsTrue(gene1.Equals(gene2));
            }

            [TestMethod]
            public void DifferentEnableTest()
            {
                ConnectionGene gene1 = new ConnectionGene(Guid.NewGuid(), 1, 0, 2, 1, true);
                ConnectionGene gene2 = new ConnectionGene(Guid.NewGuid(), 1, 0, 2, 1, false);
                Assert.IsFalse(gene1.Equals(gene2));
            }

            [TestMethod]
            public void DifferentInnovationTest()
            {
                ConnectionGene gene1 = new ConnectionGene(Guid.NewGuid(), 1, 0, 2, 1, true);
                ConnectionGene gene2 = new ConnectionGene(Guid.NewGuid(), 2, 0, 2, 1, true);
                Assert.IsFalse(gene1.Equals(gene2));
            }

            [TestMethod]
            public void DifferentWeightTest()
            {
                ConnectionGene gene1 = new ConnectionGene(Guid.NewGuid(), 1, 0, 2, 1, true);
                ConnectionGene gene2 = new ConnectionGene(Guid.NewGuid(), 2, 0, 2, -1, true);
                Assert.IsFalse(gene1.Equals(gene2));
            }

            [TestMethod]
            [DataRow(5)]
            [DataRow(null)]
            [DataRow(default)]
            [DataRow("brain")]
            [DataRow(new int[] { 1, 2, 3 })]
            public void DifferentTypeTest(object other)
            {
                ConnectionGene gene1 = new ConnectionGene(Guid.NewGuid(), 1, 0, 2, 1, true);
                Assert.IsFalse(gene1.Equals(other));
            }
        }
    }
}