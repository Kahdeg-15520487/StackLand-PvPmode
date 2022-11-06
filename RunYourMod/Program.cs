using Microsoft.Win32;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;

StopStackLand();

var rootPath = Path.Combine(Directory.GetParent("..\\..\\..").ToString(), "YourMod");
var stacklandDir = GetStackLandDir(Path.Combine(rootPath, "YourMod.csproj.user"));

var pluginsPath = $"{Environment.ExpandEnvironmentVariables("%AppData%")}\\Thunderstore Mod Manager\\DataFolder\\Stacklands\\profiles\\Default\\BepInEx\\plugins";
var bepInExLoaderPath = Path.Combine(Directory.GetParent(pluginsPath).ToString(), "core\\BepInEx.Preloader.dll");
var pluginManifestPath = Path.Combine(rootPath, "manifest.json");
var pluginManifest = JsonSerializer.Deserialize<PluginManifest>(File.ReadAllText(pluginManifestPath));
var pluginFolderName = $"local-{pluginManifest.name}";
var pluginDestination = Path.Combine(pluginsPath, pluginFolderName);
var packageDestination = Path.Combine(rootPath, "TSMMPackage\\plugins");

var fileToCopy = new List<string>
{
    "icon.png",
    "README.MD",
    "manifest.json",
    "translation.tsv",
    "textures.txt",
    "Steamworks.NET.dll",
};

var directoryToCopy = new List<string>
{
    "Blueprints",
    "Cards",
    "Images",
    "Sounds",
    "UI",
};

Copy(packageDestination);
Copy(pluginDestination);

StartStackLand();

void StartStackLand()
{
    string steamDir = GetSteamDir();

    var psi = new ProcessStartInfo();
    psi.FileName = Path.Combine(steamDir, "steam.exe");
    psi.UseShellExecute = true;
    psi.Arguments = $" -applaunch 1948280 --doorstop-enable true --doorstop-target \"{bepInExLoaderPath}\"";
    Process.Start(psi);
}

string GetSteamDir()
{
    var regKey = Environment.Is64BitOperatingSystem ? @"SOFTWARE\Wow6432Node\Valve\Steam" : @"SOFTWARE\Valve\Steam";
    RegistryKey key = Registry.LocalMachine.OpenSubKey(regKey);
    var installPath = (string)key.GetValue("InstallPath");
    key.Close();
    return installPath;
}

void StopStackLand()
{
    foreach (var p in Process.GetProcessesByName("StackLands"))
    {
        p.Kill();
    }
}

void Copy(string destination)
{
    fileToCopy.ForEach(f => CopyFile(rootPath, destination, f));
    CopyFile(Path.Combine(rootPath, "bin\\Debug\\netstandard2.0"), destination, "YourMod.dll");
    directoryToCopy.ForEach(d => CopyDirectory(rootPath, destination, d));
}

void CopyFile(string sourcePath, string destinationPath, string file)
{
    Directory.CreateDirectory(destinationPath);
    File.Copy(Path.Combine(sourcePath, file), Path.Combine(destinationPath, file), true);
}

void CopyDirectory(string rootPath, string destination, string directory)
{
    CopyFilesRecursively(new DirectoryInfo(Path.Combine(rootPath, directory)), new DirectoryInfo(Path.Combine(destination, directory)));
}

void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
{
    target.Create();
    foreach (DirectoryInfo dir in source.GetDirectories())
    {
        CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
    }
    foreach (FileInfo file in source.GetFiles())
    {
        file.CopyTo(Path.Combine(target.FullName, file.Name), true);
    }
}

string GetStackLandDir(string f)
{
    foreach (XElement xElement in XElement.Load(f).Elements("PropertyGroup"))
    {
        return xElement.Element("stacklandsDir").Value;
    }
    return null;
}

public class PluginManifest
{
    public string name { get; set; }
    public string id { get; set; }
    public string description { get; set; }
    public string version_number { get; set; }
    public List<string> dependencies { get; set; }
    public string website_url { get; set; }
}

