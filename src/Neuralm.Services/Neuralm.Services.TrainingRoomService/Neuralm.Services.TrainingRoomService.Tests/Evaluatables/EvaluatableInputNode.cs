using System;
using Neuralm.Services.TrainingRoomService.Domain;

namespace Neuralm.Services.TrainingRoomService.Tests.Evaluatables
{
    public class EvaluatableInputNode : InputNode, IEvaluatableNode
    {
        private double _value;

        public EvaluatableInputNode(uint nodeIdentifier) : base(nodeIdentifier)
        {
        }

        public void SetValue(double value)
        {
            _value = value;
        }

        public double GetValue()
        {
            return _value;
        }

        public void AddDependency(EvaluatableConnectionGene connectionGene)
        {
            throw new Exception("Input nodes are not allowed to have connections from other nodes as input.");
        }
    }
}
