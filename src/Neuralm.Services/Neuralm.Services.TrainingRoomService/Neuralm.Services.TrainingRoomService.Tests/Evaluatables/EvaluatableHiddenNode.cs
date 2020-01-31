using System;
using System.Collections.Generic;
using Neuralm.Services.TrainingRoomService.Domain;

namespace Neuralm.Services.TrainingRoomService.Tests.Evaluatables
{
    public class EvaluatableHiddenNode : HiddenNode, IEvaluatableNode
    {
        private readonly List<EvaluatableConnectionGene> _connectionGenes = new List<EvaluatableConnectionGene>();
        public IReadOnlyList<EvaluatableConnectionGene> ConnectionGenes => _connectionGenes.AsReadOnly();

        public EvaluatableHiddenNode(uint nodeIdentifier) : base(nodeIdentifier)
        {
        }

        public void SetValue(double value)
        {
           throw new Exception("Can't set the value of a hidden node.");
        }

        public double GetValue()
        {
            if (ConnectionGenes.Count == 0)
                return ActivationFunction(0);
            double total = 0;
            int count = 0;

            foreach (EvaluatableConnectionGene connectionGene in _connectionGenes)
            {
                total += connectionGene.GetValue();
                count++;
            }

            return ActivationFunction(total / count);
        }

        private static double ActivationFunction(double i)
        {
            return 1 / (1 + Math.Exp(-i));
        }

        public void AddDependency(EvaluatableConnectionGene connectionGene)
        {
            _connectionGenes.Add(connectionGene);
        }
    }
}