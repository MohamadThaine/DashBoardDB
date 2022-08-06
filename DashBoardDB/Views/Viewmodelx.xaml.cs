using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MySql.Data.MySqlClient;
using System;

namespace DashBoardDB
{
    public partial class Viewmodelx
    {
        MySqlConnection connection;
        Double[] lastTenDaysProfit = new double[10];
        DateTime[] lastTenDaysDate = new DateTime[10];

        public Viewmodelx()
        {
            ManageDB manageDB = new();
            connection = manageDB.ConnectionToDB();
            if (connection != null)
            {
                try
                {
                    connection.Open();
                }
                catch (MySqlException MYSQLEX)
                {
                    //
                }
                GetLast10DaysProfit(lastTenDaysDate, lastTenDaysProfit);
            }
            int i = 9;
            var tendays = new[]
            {
                new days {Name = lastTenDaysDate[i].ToString("MM/dd") , profit = lastTenDaysProfit[i--]},
                new days {Name = lastTenDaysDate[i].ToString("MM/dd") , profit = lastTenDaysProfit[i--]},
                new days {Name = lastTenDaysDate[i].ToString("MM/dd") , profit = lastTenDaysProfit[i--]},
                new days {Name = lastTenDaysDate[i].ToString("MM/dd") , profit = lastTenDaysProfit[i--]},
                new days {Name = lastTenDaysDate[i].ToString("MM/dd") , profit = lastTenDaysProfit[i--]},
                new days {Name = lastTenDaysDate[i].ToString("MM/dd") , profit = lastTenDaysProfit[i--] },
                new days {Name = lastTenDaysDate[i].ToString("MM/dd") , profit = lastTenDaysProfit[i--]},
                new days {Name = lastTenDaysDate[i].ToString("MM/dd") , profit = lastTenDaysProfit[i--]},
                new days {Name = lastTenDaysDate[i].ToString("MM/dd") , profit = lastTenDaysProfit[i--]},
                new days {Name = lastTenDaysDate[i].ToString("MM/dd") , profit = lastTenDaysProfit[i--]}
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
        private void GetLast10DaysProfit(DateTime[] dates, Double[] profit)
        {
            MySqlCommand cmd;
            String Get10DaysProfit = "";
            for (int i = 0; i < 10; i++)
            {
                Get10DaysProfit = "SELECT SUM(ProfitFromOrder) FROM orders WHERE DATE(OrderDate) = CURRENT_DATE() - interval " + i + " day";
                using (cmd = new MySqlCommand(Get10DaysProfit, connection))
                {
                    var CheckNoProfit = cmd.ExecuteScalar();
                    if (CheckNoProfit is not DBNull)
                        profit[i] = Convert.ToDouble(CheckNoProfit);
                    dates[i] = DateTime.Today.AddDays(-i);
                }
            }
        }
    }
    public class days
    {
        public string Name { get; set; }
        public double profit { get; set; }
    }
}
