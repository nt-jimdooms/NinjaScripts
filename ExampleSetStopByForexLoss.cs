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
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Strategies in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Strategies
{
	public class ExampleSetStopByForexLoss : Strategy
	{
		private double PipSize;
		private int LotSize;
		private double USDExchangeRate;
		private double StopLossAmount;
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Strategy here.";
				Name										= "ExampleSetStopByForexLoss";
				Calculate									= Calculate.OnBarClose;
				EntriesPerDirection							= 1;
				EntryHandling								= EntryHandling.AllEntries;
				IsExitOnSessionCloseStrategy				= true;
				ExitOnSessionCloseSeconds					= 30;
				IsFillLimitOnTouch							= false;
				MaximumBarsLookBack							= MaximumBarsLookBack.TwoHundredFiftySix;
				OrderFillResolution							= OrderFillResolution.Standard;
				Slippage									= 0;
				StartBehavior								= StartBehavior.WaitUntilFlat;
				TimeInForce									= TimeInForce.Gtc;
				TraceOrders									= false;
				RealtimeErrorHandling						= RealtimeErrorHandling.StopCancelClose;
				StopTargetHandling							= StopTargetHandling.PerEntryExecution;
				BarsRequiredToTrade							= 20;
				// Disable this property for performance gains in Strategy Analyzer optimizations
				// See the Help Guide for additional information
				IsInstantiatedOnEachOptimizationIteration	= true;
				StopLossDollarAmt					= 500;
				LotSize = 10000;
			}
			else if (State == State.Configure)
			{
				// add USDCAD series so we can get the CAD to US dollars exchange rate to set our stop
				AddDataSeries("USDCAD", Data.BarsPeriodType.Minute, 1, Data.MarketDataType.Last);
			}
		}

		protected override void OnBarUpdate()
		{
			if(State == State.Historical)
				return;
			
			if(BarsInProgress == 0)
			{
				//multiply the tick size * 10 to get the size of a full pip of movement
				PipSize = TickSize * 10;
				//to get the USD exchange rate, since USDCAD isn't quoted in USD, we need to divide 1 by the close price of that secondary instrument
				//then multiply it by the size of the lot we are using and the pip size of our primary series on which we will submit our orders
				USDExchangeRate = PipSize * LotSize * (1 / Closes[1][0]);
				// we then have to divide the desired stop loss dollar amount in USD by the exchange rate times our lot size to get the necessary price offset
				StopLossAmount = StopLossDollarAmt / (USDExchangeRate * LotSize);
				
				if (Closes[0][0] > Opens[0][0] && Position.MarketPosition == MarketPosition.Flat)
				{
					//we subtract the calculated stop loss amount from the close to get the price the stop should be placed at.  
					//the price can be confirmed by using the ruler set on currency mode to get the difference in CAD and converting that to USD manually using a converter.
					SetStopLoss(CalculationMode.Price, Closes[0][0] - StopLossAmount);
					EnterLong(LotSize);
				}
			}
			
			
		}

		#region Properties
		[NinjaScriptProperty]
		[Range(1, double.MaxValue)]
		[Display(Name="StopLossDollarAmt", Order=1, GroupName="Parameters")]
		public double StopLossDollarAmt
		{ get; set; }
		#endregion

	}
}
