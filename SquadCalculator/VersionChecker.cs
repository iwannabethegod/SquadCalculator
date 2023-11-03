using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SquadCalculator;

public class VersionChecker : INotifyPropertyChanged
{
    private Visibility _visibility = Visibility.Hidden;
    public Visibility Visibility
    {
        get => _visibility;
        set
        {
            _visibility = value;
            OnPropertyChanged();
        }
    }
    

    private string _gitHubVersionUrl =
        "https://raw.githubusercontent.com/iwannabethegod/SquadCalculator/master/Calculator/Version";

    private async Task<string> GetLatestVersionFromGitHubAsync()
    {
        using (WebClient webClient = new WebClient())
        {
            return await webClient.DownloadStringTaskAsync(_gitHubVersionUrl);
        }
    }


    private bool IsNewVersionAvailable(string latestVersion)
    {
        Version currentVersion = new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        Version latest = new Version(latestVersion);

        return latest > currentVersion;
    }
    private void ReadWriteVersion()
    {
        string filePath = "Version";

        Version currentVersion = new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString());

        string fileContents = null;

        try
        {
            if (File.Exists(filePath))
            {
                fileContents = File.ReadAllText(filePath);
            }

            if (string.IsNullOrEmpty(fileContents))
            {

                File.WriteAllText(filePath, currentVersion.ToString());
            }
            else
            {
                Version fileVersion = new Version(fileContents);

                if (currentVersion > fileVersion)
                {
                    File.WriteAllText(filePath, currentVersion.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            //
        }
    }

    public async Task CheckForUpdateAsync()
    {
        ReadWriteVersion();
        try
        {
            string latestVersion = await GetLatestVersionFromGitHubAsync();
           
            if (IsNewVersionAvailable(latestVersion))
            {
                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Hidden;
            }
        }
        catch (Exception ex)
        {
            //;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}