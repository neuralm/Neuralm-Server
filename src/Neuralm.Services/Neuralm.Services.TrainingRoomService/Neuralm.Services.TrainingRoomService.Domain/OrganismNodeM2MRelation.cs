﻿using Neuralm.Services.Common.Domain;
using System;

namespace Neuralm.Services.TrainingRoomService.Domain
{
    public class OrganismInputNode : IEquatable<OrganismInputNode>, IEntity
    {
        public Guid Id { get; set; }

        public OrganismInputNode() //EF Core
        {

        }

        public OrganismInputNode(Organism organism, InputNode inputNode)
        {
            OrganismId = organism.Id;
            Organism = organism;

            InputNodeId = inputNode.Id;
            InputNode = inputNode;
        }

        public Guid OrganismId { get; set; }
        public virtual Organism Organism { get; set; }

        public Guid InputNodeId { get; set; }
        public virtual InputNode InputNode { get; set; }


        public bool Equals(OrganismInputNode other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Organism.Equals(other.Organism, true) && InputNode.Equals(other.InputNode);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return Equals(obj as OrganismInputNode);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (OrganismId.GetHashCode() * 397) ^ InputNodeId.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"OrganismId: {OrganismId}, InputNodeId: {InputNodeId}";
        }
    }

    public class OrganismOutputNode : IEquatable<OrganismOutputNode>, IEntity
    {
        public Guid Id { get; set; }

        public OrganismOutputNode() //EF Core
        {

        }

        public OrganismOutputNode(Organism organism, OutputNode outputNode)
        {
            OrganismId = organism.Id;
            Organism = organism;

            OutputNodeId = outputNode.Id;
            OutputNode = outputNode;
        }

        public Guid OrganismId { get; set; }
        public virtual Organism Organism { get; set; }

        public Guid OutputNodeId { get; set; }
        public virtual OutputNode OutputNode { get; set; }

        public bool Equals(OrganismOutputNode other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Organism.Equals(other.Organism, true) && OutputNode.Equals(other.OutputNode);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return Equals(obj as OrganismOutputNode);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (OrganismId.GetHashCode() * 397) ^ OutputNodeId.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"OrganismId: {OrganismId}, OutputNodeId: {OutputNodeId}";
        }
    }
}
