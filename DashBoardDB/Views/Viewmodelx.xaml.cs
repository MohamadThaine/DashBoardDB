using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Windows.Media;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace DashBoardDB
{
    [ObservableObject]
    public partial class Viewmodelx
    {
        public Viewmodelx()
        {
            var tendays = new[]
            {
                new days {Name = "1/6" , profit = 5},
                new days {Name = "2/6" , profit = 10},
                new days {Name = "3/6" , profit = 15},
                new days {Name = "4/6" , profit = 10},
                new days {Name = "5/6" , profit = 20},
                new days {Name = "6/6" , profit = 35},
                new days {Name = "7/6" , profit = 30},
                new days {Name = "8/6" , profit = 20},
                new days {Name = "9/6" , profit = 50},
                new days {Name = "10/6" , profit = 70}
            };
            SeriesCollection = new[]{
                              new LineSeries<days>
                              {
                                  Name = "profit each day",
                                  TooltipLabelFormatter = point => $"{point.Model.Name} {point.PrimaryValue:N2}$",
                                  Values = tendays,
                                  Mapping = (days , points) =>
                                  {
                                      points.PrimaryValue = (double)days.profit;
                                      points.SecondaryValue = points.Context.Index + 1;
                                  }
                              }
                              };
        }
            
        public ISeries[] SeriesCollection { get; set; }
    }
    public class days
    {
        public string Name { get; set; }
        public double profit { get; set; }
    }
}
