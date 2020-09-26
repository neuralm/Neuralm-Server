using System;

namespace Neuralm.Services.TrainingRoomService.Domain.Evaluatables
{
    public class EvaluatableConnectionGene : ConnectionGene
    {
        public EvaluatableConnectionGene(Guid organismId, uint innovationNumber, uint inNodeId, uint outNodeId, double weight, bool enabled = true) : 
            base(organismId, innovationNumber, inNodeId, outNodeId, weight, enabled)
        {
        }

        public void BuildStructure(EvaluatableOrganism organism)
        {
            if (!organism.Id.Equals(OrganismId))
                throw new Exception("The buildStructure function was passed a different organism then its OrganismId.");

            InNode = organism.GetNodeFromIdentifier(InNodeIdentifier);
            OutNode = organism.GetNodeFromIdentifier(OutNodeIdentifier);

            if (!Enabled) 
                return;
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
            return ((IEvaluatableNode) InNode).GetValue() * Weight;
        }

        public override ConnectionGene Clone(Guid organismId)
        {
            return new EvaluatableConnectionGene(organismId, InnovationNumber, InNodeIdentifier, OutNodeIdentifier, Weight, Enabled);
        }
    }
}