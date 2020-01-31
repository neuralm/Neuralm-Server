using System;

namespace Neuralm.Services.TrainingRoomService.Tests.Evaluatables
{
    public class Xor
    {
        public void Test(EvaluatableOrganism evaluatableOrganism)
        {
            double error = 0;
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    double[] output = evaluatableOrganism.Evaluate(new double[] {i, j});
                    double expected = i ^ j;
                    error += Math.Abs(expected - output[0]);
                }
            }

            double score = 4 - error;
            evaluatableOrganism.Score = score;
        }
    }
}
