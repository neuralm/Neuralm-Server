﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Tests
{
    [TestClass]
    public class ConnectionGeneTests
    {
        [TestClass]
        public class CloneTest
        {
            private ConnectionGene _original;

            [TestInitialize]
            public void Initialize()
            {
                _original = new ConnectionGene(0, 2, 0.5, 1, true);
            }

            [TestMethod]
            public void CloningTest()
            {
                ConnectionGene clone = _original.Clone();

                Assert.AreEqual(_original, clone);
            }

            [TestMethod]
            public void CloneDoesntAffectOriginalTest()
            {
                double originalWeight = _original.Weight;
                ConnectionGene clone = _original.Clone();
                clone.Weight = 9001;

                Assert.AreEqual(_original.Weight, originalWeight);
            }
        }

        [TestClass]
        public class EqualsTest
        {
            [TestMethod]
            public void AreEqualTest()
            {
                ConnectionGene gene1 = new ConnectionGene(0, 2, 1, 1, true);

                ConnectionGene gene2 = new ConnectionGene(0, 2, 1, 1, true);

                Assert.IsTrue(gene1.Equals(gene2));
            }

            [TestMethod]
            public void DifferentEnableTest()
            {
                ConnectionGene gene1 = new ConnectionGene(0, 2, 1, 1, true);

                ConnectionGene gene2 = new ConnectionGene(0, 2, 1, 1, false);

                Assert.IsFalse(gene1.Equals(gene2));
            }

            [TestMethod]
            public void DifferentInnovationTest()
            {
                ConnectionGene gene1 = new ConnectionGene(0, 2, 1, 1, true);

                ConnectionGene gene2 = new ConnectionGene(0, 2, 1, 2, true);

                Assert.IsFalse(gene1.Equals(gene2));
            }

            [TestMethod]
            public void DifferentWeightTest()
            {
                ConnectionGene gene1 = new ConnectionGene(0, 2, 1, 1, true);

                ConnectionGene gene2 = new ConnectionGene(0, 2, -1, 2, true);

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
                ConnectionGene gene1 = new ConnectionGene(0, 2, 1, 1, true);

                Assert.IsFalse(gene1.Equals(other));
            }
        }
    }
}
