using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPivotGrid.Customization;
using System.Web.UI.WebControls;
using DotNet.Highcharts;
using Pivot.Models;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Helpers;

namespace Pivot.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Demo()
        {
            return View();
        }

     



        //[ValidateInput(false)]
        //public ActionResult PivotGrid2Partial()
        //{
        //    var model = db.Invoices;
        //    return PartialView("_PivotGrid2Partial", model.ToList());
        //}

        //[ValidateInput(false)]
        //public ActionResult PivotGrid3Partial()
        //{
        //    var model = new object[0];
        //    return PartialView("_PivotGrid3Partial", model);
        //}




        //public ActionResult Edit()
        //{
        //    return View(db.GetSalesPerson());
        //}

        public ActionResult PivotPartial()
        {
            return PartialView(NorthwindDataProvider.GetSalesPerson());
        }


        public ActionResult CustomizationPartial()
        {
            return PartialView(NorthwindDataProvider.GetSalesPerson());
        }

        public ActionResult ChartPartial()
        {
            return PartialView((DotNet.Highcharts.Highcharts)Session["Chart"]);
        }

        public ActionResult DummyChart()
        {
            PivotGridSettings pivotGridSettings = Pivot.Controllers.DataHelper.ChartDashletPivotGridSettings;
            DotNet.Highcharts.Highcharts chart = Pivot.Controllers.DataHelper.GetHighCharts((DevExpress.Web.ASPxPivotGrid.PivotChartDataSource)PivotGridExtension.GetDataObject(pivotGridSettings,
                NorthwindDataProvider.GetSalesPerson()));

            Session["Chart"] = chart;

            return PartialView();
        }






	}

    public class DataHelper
    {
        static PivotGridSettings pgSettings;
        public static PivotGridSettings ChartDashletPivotGridSettings
        {
            get
            {
                if (pgSettings == null)
                    pgSettings = CreateChartDashletPivotGridSettings();
                return pgSettings;
            }
        }
        static PivotGridSettings CreateChartDashletPivotGridSettings()
        {
            PivotGridSettings settings = new PivotGridSettings();
            settings.Name = "pivotGrid";
            settings.CallbackRouteValues = new { Controller = "Home", Action = "PivotPartial" };
            settings.OptionsPager.NumericButtonCount = 7;
            settings.OptionsPager.RowsPerPage = 15;
            settings.OptionsView.ShowHorizontalScrollBar = false;
            settings.OptionsView.RowTotalsLocation = PivotRowTotalsLocation.Tree;
            settings.OptionsView.ShowTotalsForSingleValues = true;
            settings.OptionsView.ShowColumnHeaders = false;
            settings.OptionsView.ShowDataHeaders = false;
            settings.OptionsView.ShowFilterHeaders = false;
            settings.OptionsView.ShowRowHeaders = false;
            settings.Width = Unit.Pixel(400);
            //settings.Height = Unit.Pixel(1);
            settings.PivotCustomizationExtensionSettings.Name = "pivotCustomization";
            settings.PivotCustomizationExtensionSettings.AllowedLayouts = CustomizationFormAllowedLayouts.BottomPanelOnly1by4 | CustomizationFormAllowedLayouts.BottomPanelOnly2by2 |
                CustomizationFormAllowedLayouts.StackedDefault | CustomizationFormAllowedLayouts.StackedSideBySide;
            settings.PivotCustomizationExtensionSettings.Layout = CustomizationFormLayout.BottomPanelOnly2by2;
            settings.PivotCustomizationExtensionSettings.AllowSort = true;
            settings.PivotCustomizationExtensionSettings.AllowFilter = true;
            settings.PivotCustomizationExtensionSettings.Height = Unit.Pixel(480);
            settings.PivotCustomizationExtensionSettings.Width = Unit.Pixel(400);

            settings.ClientSideEvents.BeginCallback = "OnBeforePivotGridCallback";
            settings.ClientSideEvents.EndCallback = "UpdateChart";

            settings.Fields.Add(field =>
            {
                field.FieldName = "Country";
                field.Area = PivotArea.RowArea;
                field.AreaIndex = 0;
            });

            settings.Fields.Add("Quantity", PivotArea.DataArea);
            settings.Fields.Add("CategoryName", PivotArea.ColumnArea);
            settings.Fields.Add("ProductName", PivotArea.FilterArea);


            return settings;
        }

        public static Highcharts GetHighCharts(DevExpress.Web.ASPxPivotGrid.PivotChartDataSource pcds)
        {
            DevExpress.Web.ASPxPivotGrid.PivotChartDataSource chartModel = pcds;

            List<String> xAxisLables = new List<string>();
            List<Series> seriesList = new List<Series>();
            for (int i = -0; i < chartModel.Count; i++)
            {
                DevExpress.Web.ASPxPivotGrid.PivotChartDataSourceRow row = (DevExpress.Web.ASPxPivotGrid.PivotChartDataSourceRow)chartModel.GetRowItem(i);

                if (true)
                {
                    if (!xAxisLables.Contains(row.Series.ToString()))
                        xAxisLables.Add(row.Series.ToString());

                    Series s = null;
                    foreach (Series item in seriesList)
                    {
                        if (item.Name == row.Argument.ToString())
                        {
                            s = item;
                            break;
                        }
                    }

                    if (s == null)
                        seriesList.Add(new Series
                        {
                            Name = row.Argument.ToString(),
                            Data = new Data(new object[] { row.Value })
                        });
                    else
                    {
                        object[] oa = s.Data.ArrayData;
                        List<object> da = new List<object>();
                        for (int iq = 0; iq < oa.Count(); iq++)
                        {
                            da.Add(oa[iq]);
                        }
                        da.Add(row.Value);
                        s.Data = new Data(da.ToArray());

                    }
                }

            }


            Highcharts chart = new Highcharts("chart")
          .SetXAxis(new XAxis { Categories = xAxisLables.ToArray<string>() })
          .SetSeries(seriesList.ToArray<Series>());
            return chart;
        }
    }
}