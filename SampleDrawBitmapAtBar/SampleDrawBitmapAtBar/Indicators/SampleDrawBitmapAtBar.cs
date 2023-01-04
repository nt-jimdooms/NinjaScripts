namespace NinjaTrader.NinjaScript.Indicators
{
	public class SampleDrawBitmapAtBar : Indicator
	{
		private SharpDX.Direct2D1.Bitmap myBitmap;
		private SharpDX.IO.NativeFileStream fileStream;
		private SharpDX.WIC.BitmapDecoder bitmapDecoder;
		private SharpDX.WIC.FormatConverter converter;
		private SharpDX.WIC.BitmapFrameDecode frame;
		
		private string FileName;
		private bool NeedToUpdate;
		
		// Create Series<bool> to tell if we want to draw something for a particular bar
		private Series<bool> DrawImageSeries;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Name = "SampleDrawBitmapAtBar";
				Calculate = Calculate.OnBarClose;
				IsOverlay = true;
				ScaleJustification = NinjaTrader.Gui.Chart.ScaleJustification.Right;
				IsSuspendedWhileInactive = true;
				FileName = "SampleDrawBitmap.png";

			}
			else if (State == State.DataLoaded)
			{
				// Create Series<bool> to tell if we want to draw something for a particular bar
				DrawImageSeries = new Series<bool>(this, MaximumBarsLookBack.Infinite);
			}
		}

		protected override void OnBarUpdate()
		{
			if (CurrentBar < 3)
				return;
			
			DrawImageSeries[0] = true;
			
			if (DrawImageSeries[3])
				DrawImageSeries[3] = false;
		}
		
		private void UpdateImage(string fileName)
		{
			// Dispose all Render dependant resources on RenderTarget change.
			if (myBitmap != null) 		myBitmap.Dispose();
			if (fileStream != null) 	fileStream.Dispose();
			if (bitmapDecoder != null) 	bitmapDecoder.Dispose();
			if (converter != null) 		converter.Dispose();
			if (frame != null) 			frame.Dispose();
			
			if (RenderTarget == null) return;
			
			// Neccessary for creating WIC objects.
			fileStream = new SharpDX.IO.NativeFileStream(System.IO.Path.Combine(NinjaTrader.Core.Globals.UserDataDir + "templates/MyResources/", fileName), SharpDX.IO.NativeFileMode.Open, SharpDX.IO.NativeFileAccess.Read);
			// Used to read the image source file.
			bitmapDecoder = new SharpDX.WIC.BitmapDecoder(Core.Globals.WicImagingFactory, fileStream, SharpDX.WIC.DecodeOptions.CacheOnDemand);
			// Get the first frame of the image.
			frame = bitmapDecoder.GetFrame(0);
			// Convert it to a compatible pixel format.			
			converter = new SharpDX.WIC.FormatConverter(Core.Globals.WicImagingFactory);
			converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA);
			// Create the new Bitmap1 directly from the FormatConverter.
			myBitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(RenderTarget, converter);
		}


		public override void OnRenderTargetChanged()
		{
			base.OnRenderTargetChanged();
			
			UpdateImage(FileName);			
		}

		protected override void OnRender(NinjaTrader.Gui.Chart.ChartControl chartControl, NinjaTrader.Gui.Chart.ChartScale chartScale)
		{
			base.OnRender(chartControl, chartScale);
			if (RenderTarget == null || Bars == null || Bars.Instrument == null || myBitmap == null)
				return;
			
			if (NeedToUpdate)
			{
				UpdateImage(FileName);
				NeedToUpdate = false;
			}
			
			// Limit Custom Rendering to ChartBars.FromIndex and ChartBars.ToIndex
            for (int idx = ChartBars.FromIndex; idx <= ChartBars.ToIndex; idx++)
            {
				// Reference Series objects with GetValueAt(idx).
				if(DrawImageSeries.GetValueAt(idx))
				{					
					RenderTarget.DrawBitmap(myBitmap, 
											new SharpDX.RectangleF((float)ChartControl.GetXByBarIndex(ChartBars, idx) - ChartControl.Properties.BarDistance/2,
																	(float)chartScale.GetYByValue(High.GetValueAt(idx)),
																	(float)ChartControl.Properties.BarDistance,
																	(float)chartScale.GetYByValue(High.GetValueAt(idx)) - chartScale.GetYByValue(Low.GetValueAt(idx))),
											1.0f,
											SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
				}
			}
		}
		
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private SampleDrawBitmapAtBar[] cacheSampleDrawBitmapAtBar;
		public SampleDrawBitmapAtBar SampleDrawBitmapAtBar()
		{
			return SampleDrawBitmapAtBar(Input);
		}

		public SampleDrawBitmapAtBar SampleDrawBitmapAtBar(ISeries<double> input)
		{
			if (cacheSampleDrawBitmapAtBar != null)
				for (int idx = 0; idx < cacheSampleDrawBitmapAtBar.Length; idx++)
					if (cacheSampleDrawBitmapAtBar[idx] != null &&  cacheSampleDrawBitmapAtBar[idx].EqualsInput(input))
						return cacheSampleDrawBitmapAtBar[idx];
			return CacheIndicator<SampleDrawBitmapAtBar>(new SampleDrawBitmapAtBar(), input, ref cacheSampleDrawBitmapAtBar);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.SampleDrawBitmapAtBar SampleDrawBitmapAtBar()
		{
			return indicator.SampleDrawBitmapAtBar(Input);
		}

		public Indicators.SampleDrawBitmapAtBar SampleDrawBitmapAtBar(ISeries<double> input )
		{
			return indicator.SampleDrawBitmapAtBar(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.SampleDrawBitmapAtBar SampleDrawBitmapAtBar()
		{
			return indicator.SampleDrawBitmapAtBar(Input);
		}

		public Indicators.SampleDrawBitmapAtBar SampleDrawBitmapAtBar(ISeries<double> input )
		{
			return indicator.SampleDrawBitmapAtBar(input);
		}
	}
}

#endregion
