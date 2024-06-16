using Godot;
using NUnit.Framework;

namespace MagicEvolution.Tests
{
	[TestFixture]
	public class PlayerTests
	{
		private NeuralNetwork neuralNetwork;

		[SetUp]
		public void SetUp()
		{
			neuralNetwork = new NeuralNetwork(GenomeFactory.getStarterNNGenome());
		}

		[Test]
		public void TestNNMutation()
		{
			for (int i = 1; i<= 1000; i++)
			{
				neuralNetwork.mutate();
				uint rez = ((NeuralNetworkGenome)neuralNetwork.getGenomeCopy()).getVisionGenomeCopy().calcRayCount() * VisionRayData.fieldCount;
				uint verif = neuralNetwork.getInputNeuronCount() - AI_Input.nonVisionDataFieldCount;

				Assert.AreEqual(verif, rez, "Nu se potriveste input neuron count");
			}           
		}
	}
}
