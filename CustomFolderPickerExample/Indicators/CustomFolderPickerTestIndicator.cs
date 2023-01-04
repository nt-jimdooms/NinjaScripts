#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

using NinjaTrader.NinjaScript.AddOns; // Our DXMediaBrush and DXHelper classes reside in the AddOn namespace.

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class CustomFolderPickerTestIndicator : Indicator
	{			
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Demonstration script using the SharpDXHelper class for managed custom rendering";
				Name										= "CustomFolderPickerTestIndicator";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= true;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
			}
		}

		protected override void OnBarUpdate()
		{
			
		}
		
		[NinjaScriptProperty]
		[PropertyEditor("NinjaTrader.NinjaScript.AddOns.CustomFolderPicker")]
		[Display(Name="Folder Path", Description="", Order=1)]
		public string FolderPath
		{ get; set; }
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private CustomFolderPickerTestIndicator[] cacheCustomFolderPickerTestIndicator;
		public CustomFolderPickerTestIndicator CustomFolderPickerTestIndicator(string folderPath)
		{
			return CustomFolderPickerTestIndicator(Input, folderPath);
		}

		public CustomFolderPickerTestIndicator CustomFolderPickerTestIndicator(ISeries<double> input, string folderPath)
		{
			if (cacheCustomFolderPickerTestIndicator != null)
				for (int idx = 0; idx < cacheCustomFolderPickerTestIndicator.Length; idx++)
					if (cacheCustomFolderPickerTestIndicator[idx] != null && cacheCustomFolderPickerTestIndicator[idx].FolderPath == folderPath && cacheCustomFolderPickerTestIndicator[idx].EqualsInput(input))
						return cacheCustomFolderPickerTestIndicator[idx];
			return CacheIndicator<CustomFolderPickerTestIndicator>(new CustomFolderPickerTestIndicator(){ FolderPath = folderPath }, input, ref cacheCustomFolderPickerTestIndicator);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.CustomFolderPickerTestIndicator CustomFolderPickerTestIndicator(string folderPath)
		{
			return indicator.CustomFolderPickerTestIndicator(Input, folderPath);
		}

		public Indicators.CustomFolderPickerTestIndicator CustomFolderPickerTestIndicator(ISeries<double> input , string folderPath)
		{
			return indicator.CustomFolderPickerTestIndicator(input, folderPath);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.CustomFolderPickerTestIndicator CustomFolderPickerTestIndicator(string folderPath)
		{
			return indicator.CustomFolderPickerTestIndicator(Input, folderPath);
		}

		public Indicators.CustomFolderPickerTestIndicator CustomFolderPickerTestIndicator(ISeries<double> input , string folderPath)
		{
			return indicator.CustomFolderPickerTestIndicator(input, folderPath);
		}
	}
}

#endregion
