using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using SquadCalculator.Models;
using SquadCalculator.ViewModels;

namespace SquadCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            MainViewModel.Maps.LoadMapsAsync();
            
        }
       
        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
           MapControl.MouseMove(sender, e);
        }

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MapControl.MouseLeftButtonDown(sender, e);
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MapControl.MouseLeftButtonUp(sender, e);
        }
        

        private void CanvasMouseRighButtonDown(object sender, MouseButtonEventArgs e)
        {
            MapControl.MouseRightButtonDown(sender,e);
        }
        
        private void CanvasMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MapControl.MouseWheel(sender, e);
        }

        private void CanvasDrawingArea_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (MapsList.SelectedItem != null)
            {
                MapControl.OnMouseLeave(sender, new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left));
            }
        }

        private void MapsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MapControl.SetProperties(CanvasDrawingArea, OuterCanvas, CanvasTransform, CanvasScale, HeightMapImage);
            MapControl.SetCanvasStartPosition();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            string url = ((Hyperlink)sender).NavigateUri.ToString();
            
            Process.Start(
                new ProcessStartInfo()
                    { FileName = url, 
                        UseShellExecute = true });
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            string url = "https://www.buymeacoffee.com/iwannabethegod";
            Process.Start(
                new ProcessStartInfo()
                    { FileName = url, 
                        UseShellExecute = true });
        }
    }
}
