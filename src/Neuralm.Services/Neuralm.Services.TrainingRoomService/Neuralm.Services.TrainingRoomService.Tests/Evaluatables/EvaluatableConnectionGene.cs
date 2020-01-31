using System;
using System.Diagnostics;
using Neuralm.Services.TrainingRoomService.Domain;

namespace Neuralm.Services.TrainingRoomService.Tests.Evaluatables
{
    public class EvaluatableConnectionGene : ConnectionGene
    {
        public EvaluatableConnectionGene(Guid organismId, uint innovationNumber, uint inNodeId, uint outNodeId, double weight, bool enabled = true) : 
            base(organismId, innovationNumber, inNodeId, outNodeId, weight, enabled)
        {
        }

        public void BuildStructure(EvaluatableOrganism organism)
        {
            if (!Enabled)
                return;

            if (!organism.Id.Equals(OrganismId))
                throw new Exception("The buildStructure function was passed a different organism then its OrganismId.");

            InNode = organism.GetNodeFromIdentifier(InNodeIdentifier);
            if (InNode is null)
                throw new ArgumentOutOfRangeException();
            OutNode = organism.GetNodeFromIdentifier(OutNodeIdentifier);
            switch (OutNode)
            {
                case EvaluatableOutputNode eo:
                    eo.AddDependency(this);
                    break;
                case EvaluatableHiddenNode eh:
                    eh.AddDependency(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public double GetValue()
        {
            if (InNode is null)
                throw new NullReferenceException("The input node is default");
            return ((EvaluatableInputNode) InNode).GetValue() * Weight;
        }

        public override ConnectionGene Clone(Guid organismId)
        {
            return new EvaluatableConnectionGene(organismId, InnovationNumber, InNodeIdentifier, OutNodeIdentifier, Weight, Enabled);
        }
    }
}