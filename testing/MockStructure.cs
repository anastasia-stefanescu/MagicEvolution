using Godot;
using System.Collections.Generic;
public partial class MockNode : Node
{
	private List<Node> _children = new List<Node>();
	public void AddMockChild(Node node)
	{
		_children.Add(node);
		node._Ready();  // Simulate the _Ready method being called when a node is added
	}

	public void ProcessMock(float delta)
	{
		foreach (var child in _children)
		{
			child._Process(delta); // Simulate processing each child node
		}
	}
}

public partial class MockSceneTree : SceneTree
{
	private MockNode _rootNode;

	public MockSceneTree()
	{
		_rootNode = new MockNode();
	}

	public MockNode Root => _rootNode;

	public void Process(float delta)
	{
		_rootNode.ProcessMock(delta);
	}
}
