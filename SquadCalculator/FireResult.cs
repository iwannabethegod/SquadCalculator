﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SquadCalculator;

public class FireResult : INotifyPropertyChanged
{
    private string _distance;
    private string _azimuth;
    private string _elevation;
    private string _elevationLow;
    public string Distance
    {
        get => _distance;
        set
        {
            _distance = value;
            OnPropertyChanged();
        }
    }

    public string Azimuth
    {
        get => _azimuth;
        set
        {
            _azimuth = value ;
            OnPropertyChanged();
        }
    }

    public string Elevation
    {
        get => _elevation;
        set
        {
            _elevation = value;
            OnPropertyChanged();
        }
    }
    public string ElevationLow
    {
        get => _elevationLow;
        set
        {
            if (value == "|| 0")
            {
                _elevationLow = " ";
            }
            else
            {
                _elevationLow = value;
            }
       
            OnPropertyChanged();
        }
    }

    public FireResult(string distance, string azimuth, string elevation, string elevationLow)
    {
        Distance = distance;
        Azimuth = azimuth;
        Elevation = elevation;
        ElevationLow = elevationLow;
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

    public override string ToString()
    {
        return $"FireResult: {Distance} {Azimuth} {Elevation} {ElevationLow}";
    }
}