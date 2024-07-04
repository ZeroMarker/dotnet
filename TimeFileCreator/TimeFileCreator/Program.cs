using System;
using System.IO;
using Microsoft.Win32;

namespace AddContextMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                // Get the directory path from the argument
                string directoryPath = args[0];

                // Generate the file name with current date and time
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                string fileName = $"{timestamp}.txt";
                string filePath = Path.Combine(directoryPath, fileName);

                // Ensure the file does not already exist
                if (!File.Exists(filePath))
                {
                    // Create the new text file
                    File.WriteAllText(filePath, string.Empty);
                    Console.WriteLine($"Text file created successfully: {fileName}");
                }
                else
                {
                    Console.WriteLine("File already exists.");
                }
            }
            else
            {
                // Add context menu item
                AddContextMenuItem();
            }
        }

        static void AddContextMenuItem()
        {
            // Registry key for context menu
            string key = @"Directory\Background\shell\CreateTextFile";
            string commandKey = @"Directory\Background\shell\CreateTextFile\command";

            try
            {
                // Create the key for the context menu item
                using (RegistryKey registryKey = Registry.ClassesRoot.CreateSubKey(key))
                {
                    if (registryKey != null)
                    {
                        registryKey.SetValue("", "Create Text File");
                    }
                }

                // Create the command key and set the command to execute
                using (RegistryKey registryCommandKey = Registry.ClassesRoot.CreateSubKey(commandKey))
                {
                    if (registryCommandKey != null)
                    {
                        string executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                        registryCommandKey.SetValue("", $"\"{executablePath}\" \"%V\"");
                    }
                }

                Console.WriteLine("Context menu item added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
