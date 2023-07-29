using System.Diagnostics;
using System.Net;
using Octokit;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace AutoRobloxFPSUnlocker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!Config.Default.InStartup)
            {
                Logger.LogInfo("Do you want to add the program to startup Y/N?");
                var input = Console.ReadLine();
                if (input.ToLower() == "y")
                {
                    Config.Default.InStartup = true;
                    Config.Default.Save();
                    Logger.LogInfo("Added to startup!");
                    AddToStartup();
                }
                else
                {
                    Config.Default.InStartup = false;
                    Config.Default.Save();
                    Logger.LogInfo("Not added to startup!");
                }
            }
            if (args.Length > 0)
            {
                if (args[0] == "--self")
                {
                    UpdateSelf();
                }

                if (args[0] == "--info")
                {
                    Logger.LogInfo("AutoRobloxFPSUnlocker");
                    Logger.LogSuccess("Made by mart111n (discord)");
                    Logger.LogSuccess("Mostly for convenience, actual effort by axstin (github)");

                    Console.WriteLine();
                    Logger.LogCritical($"Version Installed: {Config.Default.InstalledVersion}");
                    Logger.LogCritical($"In Startup: {Config.Default.InStartup}");
                    Logger.LogInfo("Press any key to exit.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            else
            {
                new FpsUnlocker().Start();
            }
        }

        private static void AddToStartup()
        {
            // Add a batch file to startup that runs the program
            var a = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var batchFile = Path.Combine(a, "AutoRobloxFPSUnlocker.bat");
            File.WriteAllText(batchFile, $"\"{Path.Combine(Environment.CurrentDirectory, "AutoRobloxFPSUnlocker.exe")}\"");
        }

        internal static void UpdateSelf()
        {

        }
    }

    internal class FpsUnlocker
    {
        public void Start()
        {
            Logger.LogInfo("Starting...");
            Logger.LogInfo("Checking for updates...");
            CheckForUpdates();
            Logger.LogInfo("Checking for updates... Done!");
            Logger.LogInfo("Starting FPS Unlocker...");
            StartFpsUnlocker();
        }

        private void StartFpsUnlocker()
        {
            Process.Start("rbxfpsunlocker.exe");
        }

        private void CheckForUpdates()
        {
            if (!File.Exists("rbxfpsunlocker.exe"))
            {
                UpdateFpsUnlocker();
            }
            else
            {
                var currentVersion = Config.Default.InstalledVersion;
                var latestVersion = Convert.ToInt32(GetLatestVersion().Replace("v", "").Replace(".", ""));
                if (currentVersion != latestVersion)
                {
                    Logger.LogInfo("New version available!");
                    Logger.LogInfo($"Current version: {currentVersion}");
                    Logger.LogInfo($"Latest version: {latestVersion}");
                    Logger.LogInfo("Updating...");
                    UpdateFpsUnlocker();
                }
                else
                {
                    Logger.LogInfo("No updates available!");
                }
            }
        }

        private void UpdateFpsUnlocker()
        {
            var client = new GitHubClient(new ProductHeaderValue("AutoRobloxFPSUnlocker"));
            var releases = client.Repository.Release.GetAll("axstin", "rbxfpsunlocker").Result;
            var latestRelease = releases[0];
            var assets = latestRelease.Assets;
            var asset = assets[0];
            var downloadUrl = asset.BrowserDownloadUrl;
            var fileName = asset.Name;
            var filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            var webClient = new WebClient();
            webClient.DownloadFile(downloadUrl, filePath);
            Logger.LogInfo("Updated!");
            Config.Default.InstalledVersion = Convert.ToInt32(GetLatestVersion().Replace("v", "").Replace(".", ""));
            Config.Default.Save();
            var a = ZipArchive.Open(fileName, new ReaderOptions());
            a.WriteToDirectory(Environment.CurrentDirectory, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
            a.Dispose();
            File.Delete(fileName);
            Logger.LogSuccess("Updated & Extracted!");
        }

        private string GetLatestVersion()
        {
            var client = new GitHubClient(new ProductHeaderValue("AutoRobloxFPSUnlocker"));
            var releases = client.Repository.Release.GetAll("axstin", "rbxfpsunlocker").Result;
            var latestRelease = releases[0];
            return latestRelease.TagName;
        }
    }
}