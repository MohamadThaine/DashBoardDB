using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DashBoardDB
{
    [ObservableObject]
    public partial class Piechart
    {
        public ISeries[] Series { get; set; } = new ISeries[]
        {
        new PieSeries<double> { Values = new List<double> { 2 }, InnerRadius = 50, Name = "drink"},
        new PieSeries<double> { Values = new List<double> { 4 }, InnerRadius = 50, Name = "food" },
        new PieSeries<double> { Values = new List<double> { 1 }, InnerRadius = 50, Name = "Ice Cream" },
        new PieSeries<double> { Values = new List<double> { 4 }, InnerRadius = 50, Name = "Canned food" },
        new PieSeries<double> { Values = new List<double> { 3 }, InnerRadius = 50, Name = "chips" }
        };
    }

}


