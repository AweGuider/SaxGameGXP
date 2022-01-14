using System;									// System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Collections.Generic;
using GXPEngine.OpenGL;

public class MyGame : Game
{
    LevelManager levelManager;

    public MyGame() : base(480, 270, false, true, 960, 540, true)		// Create a window that's 800x600 and NOT fullscreen
	{
        // Draw some things on a canvas:

        /*EasyDraw canvas = new EasyDraw(800, 600, false);
        canvas.Clear(Color.MediumPurple);
        canvas.Fill(Color.Yellow);
        canvas.Ellipse(width / 2, height / 2, 200, 200);
        canvas.Fill(50);
        canvas.TextSize(32);
        canvas.TextAlign(CenterMode.Center, CenterMode.Center);
        canvas.Text("Welcome!", width / 2, height / 2);*/

        //AddChild(canvas);

        levelManager = new LevelManager(new List<string> { "MapLevel1.tmx", "MapLevel2.tmx" });
        AddChild(levelManager);

        //Sound background = new Sound("Game_Music.wav", true, true);
        //background.Play();

        Console.WriteLine("MyGame initialized");
    }

	// For every game object, Update is called every frame, by the engine:
	void Update()
	{
        levelManager.Update();
    }

	static void Main()							// Main() is the first method that's called when the program is run
	{
		new MyGame().Start();					// Create a "MyGame" and start it
	}
}