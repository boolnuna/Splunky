global using System;
global using StereoKit;

SKSettings settings = new SKSettings
{
    appName = "Splunky",
    assetsFolder = "add",
    displayPreference = DisplayMode.Flatscreen
};
if (!SK.Initialize(settings))
    Environment.Exit(1);

// Renderer.EnableSky = false;
// Renderer.ClearColor = Color.Hex(0xa9c6e2ff);
// Renderer.SetClip(0.08f, 256f);

SK.Run(() =>
{

});
    

/*

*/
