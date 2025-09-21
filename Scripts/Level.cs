using Godot;
using System;
using System.Diagnostics;

public partial class Level : Node
{
	[Export]
	public PackedScene nextLevel;
	[Export]
	Label timerLabel;
	

	private PackedScene gameOverScene;

	public static Level instance;

	public ulong startTime = 0;
	private double timeS = 0;
	
	string lastLevelName = null;
	public override void _Ready()
	{
		instance = this;
		gameOverScene = GD.Load<PackedScene>("res://Scenes/Game/GameOver.tscn");
		GD.Print("Level ready");
	}

	public void initialize(ulong time, string lastLevelName = null)
	{
		startTime = time;
		this.lastLevelName = lastLevelName;
	}
	
	public void restartLast()
	{
		if (lastLevelName != null)
		{
			var lastScene = GD.Load<PackedScene>(lastLevelName);
			Debug.Assert(lastScene != null);
			if (lastScene != null)
			{
				var nextScene = lastScene.Instantiate<Node>();

				// 2. Pass data (assuming the root script has a property or method for it)
				(nextScene as Level).initialize(startTime + (ulong)(timeS), lastLevelName);

				// 3. Replace the current scene
				var tree = GetTree();
				var currentScene = tree.CurrentScene;
				tree.Root.AddChild(nextScene);
				tree.CurrentScene = nextScene;
				currentScene.QueueFree();
			}
		}
	}

	public void gameOver()
	{
		// load game over scene
		// pass this level as parameter
		var gameOverInstance = gameOverScene.Instantiate<Node>();

		// 2. Pass data (assuming the root script has a property or method for it)
		(gameOverInstance as Level).initialize(startTime + (ulong)(timeS), GetTree().CurrentScene.SceneFilePath);

		// 3. Replace the current scene
		var tree = GetTree();
		var currentScene = tree.CurrentScene;
		tree.Root.AddChild(gameOverInstance);
		tree.CurrentScene = gameOverInstance;
		currentScene.QueueFree();
	}

	public void reachEnd()
	{
		Debug.Assert(nextLevel != null);
		if (nextLevel != null)
		{
			var nextScene = nextLevel.Instantiate<Node>();

			// 2. Pass data (assuming the root script has a property or method for it)
			(nextScene as Level).initialize(startTime + (ulong)(timeS), GetTree().CurrentScene.SceneFilePath);

			// 3. Replace the current scene
			var tree = GetTree();
			var currentScene = tree.CurrentScene;
			tree.Root.AddChild(nextScene);
			tree.CurrentScene = nextScene;
			currentScene.QueueFree();
		}
	}
	
	public override void _Process(double delta)
	{
		timeS += delta;
		if (timerLabel != null)
		{
			timerLabel.Text = ((ulong)timeS).ToString();
		}
	}
}
