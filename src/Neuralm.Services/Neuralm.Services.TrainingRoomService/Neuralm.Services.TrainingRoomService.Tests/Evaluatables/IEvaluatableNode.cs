namespace Neuralm.Services.TrainingRoomService.Tests.Evaluatables
{
    public interface IEvaluatableNode
    {
        void SetValue(double value);
        double GetValue();
        void AddDependency(EvaluatableConnectionGene connectionGene);
    }
}
