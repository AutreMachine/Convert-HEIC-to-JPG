using ImageMagick;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertHEICtoJPG
{
    public class ConvertTools
    {
        public Command Root()
        {
            var rootCommand = new Command("convertheic", " - tool to easily convert from HEIC to JPG");

            // Add commands
            rootCommand.AddCommand(getProcessCommand());

            return rootCommand;
        }

        private Command getProcessCommand()
        {
            var processCommand = new Command("process", "Process all HEIC images in the current folder and create JPG files with the same name");
            var inputOption = new Option<string?>("input", "path where the HEIC files can be found - by default current path");
            processCommand.AddOption(inputOption);
            var forceOption = new Option<bool?>("--force", "force new images to replace current ones if any - false by default");
            processCommand.AddOption(forceOption);
            var outputOption = new Option<string?>("--output", "output folder - current by default");
            processCommand.AddOption(outputOption);

            processCommand.SetHandler(async (string? path, string? output, bool? force) =>
            {
                //Console.WriteLine($"Current folder : {Directory.GetCurrentDirectory()}");
                var currentFolder = Directory.GetCurrentDirectory();
                var inputPath = path == null ? currentFolder : path; // "C:\\Users\\Utilisateur\\Pictures\\2024-12-15";
                var outputPath = output == null ? inputPath : output; // "F:\\temp\\Ventes vetements";
                string[] allfiles = Directory.GetFiles(inputPath, "*.heic", SearchOption.TopDirectoryOnly);

                int n = 0;
                foreach (var file in allfiles)
                {
                    FileInfo info = new FileInfo(file);
                    var outputName = $"{info.Name.Replace(".heic", ".jpg", true, System.Globalization.CultureInfo.CurrentCulture)}";
                    var fullOutput = Path.Combine(outputPath, outputName);
                    if ((!force.HasValue || (force.HasValue && !force.Value)) && File.Exists(fullOutput))
                    {
                        Console.WriteLine($"File {outputName} already exists : don't replace.");
                    }
                    else
                    {
                        using (MagickImage image = new MagickImage(info.FullName))
                        {
                            // Save frame as jpg
                            Console.WriteLine($"Writing {outputName} to {outputPath}...");
                            image.Write(fullOutput);
                            n++;

                        }
                    }
                }

                if (allfiles.Length == 0)
                {
                    Console.WriteLine("No HEIC image found in the folder.");
                }
                else
                {
                    Console.WriteLine($"Created {n} image(s).");
                }

            }, inputOption, outputOption, forceOption);

            return processCommand;
        }
    }
}
