using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DashBoardDB
{
    public class Allviews
    {
        public Viewmodelx Viewmodelx { get; set; }  = new Viewmodelx();
        public Piechart Piechart { get; set; } = new Piechart();
        public Bars Bars { get; set; } = new Bars();
    }
     
}
