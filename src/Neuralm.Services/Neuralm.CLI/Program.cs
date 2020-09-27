using System;
using Neuralm.Services.Common.Patterns;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Domain.Evaluatables;
using Neuralm.Services.TrainingRoomService.Domain.FactoryArguments;

namespace Neuralm.CLI
{
    public class Program
    {
        private static TrainingRoom _trainingRoom;
        private static IFactory<Organism, OrganismFactoryArgument> _organismFactory;
        private static User _fakeUser;
        private static string _roomName;
        
        public static void Main(string[] args)
        {
            Setup();
                
            Xor xor = new Xor();
            //Run 15 generations
            for (int i = 0; i < 10000; i++)
            {
                _trainingRoom.Species.ForEach(species => species.Organisms.ForEach(o =>
                {
                    xor.Test((EvaluatableOrganism) o);
                    o.IsEvaluated = true;
                }));
                _trainingRoom.EndGeneration(o => { }, o => { });
                Console.WriteLine($"Gen: {i}, TotalScore: {_trainingRoom.TotalScore}, HighestOrganismScore: {_trainingRoom.HighestOrganismScore}, LowestOrganismScore: {_trainingRoom.LowestOrganismScore}, Species: {_trainingRoom.Species.Count}");
            }
        }

        private static void Setup()
        {
            _fakeUser = new User();
            Guid trainingRoomId = Guid.NewGuid();
            _roomName = "CoolRoom";

            //Create a training room with really high mutation settings
            TrainingRoomSettings trainingRoomSettings = new TrainingRoomSettings(trainingRoomId: trainingRoomId,
                                                                                 organismCount: 50,
                                                                                 inputCount: 3,
                                                                                 outputCount: 1,
                                                                                 c1: 1,
                                                                                 c2: 1,
                                                                                 c3: 0.4,
                                                                                 threshold: 3,
                                                                                 addConnectionChance: 0.2,
                                                                                 addNodeChance: 1,
                                                                                 crossOverChance: 0.75,
                                                                                 interSpeciesChance: 0.002,
                                                                                 mutationChance: 0.3,
                                                                                 mutateWeightChance: 0.8,
                                                                                 weightReassignChance: 0.1,
                                                                                 topAmountToSurvive: 0.5,
                                                                                 enableConnectionChance: 0.25,
                                                                                 seed: 1,
                                                                                 maxStagnantTime: 5,
                                                                                 championCloneMinSpeciesSize: 5);
            _organismFactory = new EvaluatableOrganismFactory();
            _trainingRoom = new TrainingRoom(trainingRoomId, _fakeUser, _roomName, trainingRoomSettings, _organismFactory);

            for (int i = 0; i < trainingRoomSettings.OrganismCount; i++)
            {
                EvaluatableOrganism organism = new EvaluatableOrganism(trainingRoomSettings, _trainingRoom.GetInnovationNumber, 0) { IsLeased = true };
                _trainingRoom.AddOrganism(organism);
            }
            _trainingRoom.IncreaseNodeIdTo(trainingRoomSettings.InputCount + trainingRoomSettings.OutputCount);
        }
    }
}