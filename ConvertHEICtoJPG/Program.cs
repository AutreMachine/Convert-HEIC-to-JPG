using ConvertHEICtoJPG;
using ImageMagick;
using System.CommandLine;

var tools = new ConvertTools();
var rootCommand =  tools.Root();

// Run the command
return await rootCommand.InvokeAsync(args);


