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

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class CaptureChartTraderAccountSubEvents : Indicator
	{
		private NinjaTrader.Gui.Tools.AccountSelector myAccountSelector;
		private Account LastAccount, ThisAccount;
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "CaptureChartTraderAccountSubEvents";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= false;
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
			else if (State == State.DataLoaded)
			{
				if (ChartControl != null)
					ChartControl.Dispatcher.InvokeAsync(new Action(() => {
						myAccountSelector = Window.GetWindow(ChartControl.Parent).FindFirst("ChartTraderControlAccountSelector") as NinjaTrader.Gui.Tools.AccountSelector;
						
						if (myAccountSelector.SelectedAccount != null)
						{
			                ThisAccount = myAccountSelector.SelectedAccount;
							ThisAccount.AccountItemUpdate   += OnAccountItemUpdate;
						}
						
						myAccountSelector.SelectionChanged += (o, args) =>
				        {
							if (myAccountSelector.SelectedAccount != null && ThisAccount.Name != myAccountSelector.SelectedAccount.Name)
							{
								if (ThisAccount != null)
									ThisAccount.AccountItemUpdate -= OnAccountItemUpdate;
								if (LastAccount != null)
									LastAccount.AccountItemUpdate -= OnAccountItemUpdate;
								if (ThisAccount != null)
									LastAccount = ThisAccount;
								ThisAccount = myAccountSelector.SelectedAccount;
								ThisAccount.AccountItemUpdate   += OnAccountItemUpdate;
							}
				        };
    				}));
			}
			else if (State == State.Terminated)
			{				
				if (ThisAccount != null)
					ThisAccount.AccountItemUpdate -= OnAccountItemUpdate;
				
				if (LastAccount != null)
					LastAccount.AccountItemUpdate -= OnAccountItemUpdate;
			}
		}
		
		private void OnAccountItemUpdate(object sender, AccountItemEventArgs e)
	    {
	         // Output the account item
	         NinjaTrader.Code.Output.Process(string.Format("Account: {0} AccountItem: {1} Value: {2}",
	              e.Account.Name, e.AccountItem, e.Value), PrintTo.OutputTab1);
	    }

		protected override void OnBarUpdate()
		{
			//Add your custom indicator logic here.
		}
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private CaptureChartTraderAccountSubEvents[] cacheCaptureChartTraderAccountSubEvents;
		public CaptureChartTraderAccountSubEvents CaptureChartTraderAccountSubEvents()
		{
			return CaptureChartTraderAccountSubEvents(Input);
		}

		public CaptureChartTraderAccountSubEvents CaptureChartTraderAccountSubEvents(ISeries<double> input)
		{
			if (cacheCaptureChartTraderAccountSubEvents != null)
				for (int idx = 0; idx < cacheCaptureChartTraderAccountSubEvents.Length; idx++)
					if (cacheCaptureChartTraderAccountSubEvents[idx] != null &&  cacheCaptureChartTraderAccountSubEvents[idx].EqualsInput(input))
						return cacheCaptureChartTraderAccountSubEvents[idx];
			return CacheIndicator<CaptureChartTraderAccountSubEvents>(new CaptureChartTraderAccountSubEvents(), input, ref cacheCaptureChartTraderAccountSubEvents);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.CaptureChartTraderAccountSubEvents CaptureChartTraderAccountSubEvents()
		{
			return indicator.CaptureChartTraderAccountSubEvents(Input);
		}

		public Indicators.CaptureChartTraderAccountSubEvents CaptureChartTraderAccountSubEvents(ISeries<double> input )
		{
			return indicator.CaptureChartTraderAccountSubEvents(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.CaptureChartTraderAccountSubEvents CaptureChartTraderAccountSubEvents()
		{
			return indicator.CaptureChartTraderAccountSubEvents(Input);
		}

		public Indicators.CaptureChartTraderAccountSubEvents CaptureChartTraderAccountSubEvents(ISeries<double> input )
		{
			return indicator.CaptureChartTraderAccountSubEvents(input);
		}
	}
}

#endregion
