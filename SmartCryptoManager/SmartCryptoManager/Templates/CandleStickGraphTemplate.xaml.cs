using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using UltimateXF.Widget.Charts;
using UltimateXF.Widget.Charts.Models.CandleStickChart;
using UltimateXF.Widget.Charts.Models.Formatters;

namespace SmartCryptoManager.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandleStickGraphTemplate : ContentView
    {
        public static readonly BindableProperty LabelsProperty =
      BindableProperty.Create(nameof(Labels), typeof(List<string>), typeof(CandleStickGraphTemplate), null,
          propertyChanging: (binding, oldValue, newValue) =>
          {
              //var newLabelList = newValue as List<string>;

              //var targetView = (CandleStickGraphControl)binding;

              //if (targetView != null)
              //{
              //    targetView.XAxis.AxisValueFormatter = new TextByIndexXAxisFormatter(newLabelList);
              //    targetView.XAxis.ForceLabels = true;
              //}
          });

        public List<string> Labels
        {
            get => (List<string>)GetValue(LabelsProperty);
            set => SetValue(LabelsProperty, value);
        }

        public static readonly BindableProperty DataProperty =
        BindableProperty.Create(nameof(Data), typeof(List<ICandleStickDataSet>), typeof(CandleStickGraphTemplate), null,
            propertyChanged: (binding, oldValue, newValue) =>
            {
                var newDatalList = newValue as List<ICandleStickDataSet>;

                var targetView = (CandleStickGraphTemplate)binding;

                if (targetView != null && newDatalList != null && newDatalList.Count > 0)
                {

                    var control = new SupportCandleStickChartExtended();
                    control.HorizontalOptions = LayoutOptions.FillAndExpand;
                    control.VerticalOptions = LayoutOptions.FillAndExpand;

                    control.ChartData = new CandleStickChartData(newDatalList);

                    control.LogEnabled = false;
                    control.DescriptionChart.Text = string.Empty;
                    control.Legend.Enabled = false;

                    control.AxisLeft.Enabled = false;
                    control.AxisLeft.DrawAxisLine = false;
                    control.AxisLeft.DrawGridLines = false;

                    control.AxisRight.Enabled = true;
                    control.AxisRight.DrawAxisLine = false;
                    control.AxisRight.DrawGridLines = false;

                    control.XAxis.DrawGridLines = false;
                    control.DrawBorders = true;
                    control.HighlightPerDragEnabled = false;

                    control.ScaleXEnabled = true;
                    control.ScaleYEnabled = true;
                    control.PinchZoomEnabled = true;
                    control.AutoScaleMinMaxEnabled = true;

                    control.DragXEnabled = true;

                    //control.XAxis.LabelRotationAngle = 90;
                    control.XAxis.AxisValueFormatter = new TextByIndexXAxisFormatter(targetView.Labels);
                    control.XAxis.ForceLabels = true;

                    targetView.mainLayout.Children.Clear();
                    targetView.mainLayout.Children.Add(control);



                }
            });

        public List<ICandleStickDataSet> Data
        {
            get => (List<ICandleStickDataSet>)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public CandleStickGraphTemplate()
        {
            InitializeComponent();
        }
    }
}
