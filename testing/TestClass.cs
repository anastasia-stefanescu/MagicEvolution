using Godot;
using NUnit.Framework;

namespace MagicEvolution.Tests
{
	[TestFixture]
	public class PlayerTests
	{
		private Wizbit _wizbit;
		//private MockSceneTree _mockSceneTree;

		[SetUp]
		public void SetUp()
		{
			//_mockSceneTree = new MockSceneTree();

			PackedScene wizbitScene = GD.Load<PackedScene>("res://scenes/wizbit.tscn");
			_wizbit = new Wizbit(); //are automat atributele puse cum trebuie

			//GetTree().Root.CallDeferred("add_child", _wizbit);
			//_mockSceneTree.Root.AddMockChild(_wizbit);
		}

		[Test]
		public void TestWizbitInitialization()
		{
			Assert.IsNotNull(_wizbit);
			Assert.AreEqual(1, _wizbit.getId(), "Nu a fost initializat id corect");
		}

		// [Test]
		// public void TestStatsProcessing()
		// {
		//     double initialMana = _wizbit.getCurrentMana();
		//     double initialHp = _wizbit.getCurrentHp();
		//     double cost = SimulationParameters.WizbitStatsParameters.constantCost;
		//     // Simulate processing
		//     _mockSceneTree.Process(0.1f); // Simulate a frame with delta time

		//     double newHp = initialHp;
		//     if (initialHp <= _wizbit.stats.getMaxHp() - cost)
		//         newHp = initialHp + cost; 
		//     Assert.AreEqual(initialMana - cost, _wizbit.getCurrentMana(), "Nu s-a actualizat mana bine");
		//     Assert.AreEqual(newHp, _wizbit.getCurrentHp(), "Nu s-a actualizat hp bine");
		// }
	}
}
