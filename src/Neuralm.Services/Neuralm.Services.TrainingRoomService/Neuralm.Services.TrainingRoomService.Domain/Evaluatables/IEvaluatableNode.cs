namespace Neuralm.Services.TrainingRoomService.Domain.Evaluatables
{
    public interface IEvaluatableNode
    {
        void SetValue(double value);
        double GetValue();
        void AddDependency(EvaluatableConnectionGene connectionGene);
    }
}
