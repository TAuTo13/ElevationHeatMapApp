using System;
using SkiaSharp;
using MapConsoleLegend.Models;

namespace MapConsoleLegend
{
    public class LegendImgDrawer
    {
        private const string AXIS = "Height(m)";

        private readonly SKColor ALPHA = new SKColor(0, 0, 0, 0);

        private readonly SKTypeface TYPEFACE
                    = SKTypeface.FromFamilyName("Arial",
                                                SKFontStyleWeight.Normal,
                                                SKFontStyleWidth.Normal,
                                                SKFontStyleSlant.Upright);

        private readonly SKImageInfo DEFAULTINFO=new SKImageInfo(120,300);


        public ColorLegend Legend { get; set; }

        public float DrawingMargin { get; set; } = 5.0f;

        public LegendImgDrawer(ColorLegend legend)
        {
            this.Legend = legend;
        }

        public SKSurface OnDrawCanvas(SKImageInfo info,bool shouldAlpha = false)
        {
            if (info.Width < DEFAULTINFO.Width || info.Height < DEFAULTINFO.Height)
                info = DEFAULTINFO;

            var surface = SKSurface.Create(info);
            var canvas = surface.Canvas;

            if (shouldAlpha)
            {
                canvas.Clear(ALPHA);
                this.Legend.BgColor = ALPHA;
            }
            else
                canvas.Clear(SKColors.White);

            float textSize = info.Width * 0.15f;
            float maxTextSize
                = (float)info.Height / (this.Legend.ColorsLen + 1.0f)
                                                    - this.DrawingMargin * 2.0f;
            if (maxTextSize < textSize)
            {
                textSize = maxTextSize;
            }

            using (SKPaint paint = new SKPaint()
            {
                TextSize = textSize,
                Typeface = this.TYPEFACE,
                IsAntialias =true
            })
            {
                SKPoint drawingPoint = new SKPoint(DrawingMargin, DrawingMargin);

                //軸タイトル「高さ」を描画
                SKRect textBounds = new SKRect();
                paint.MeasureText(AXIS,ref textBounds);

                //drawingPoint.X -= textBounds.Left;
                drawingPoint.Y += textBounds.Height;

                canvas.DrawText(AXIS, drawingPoint, paint);

                //凡例グラデーションを描画
                int legendHeight = (int)((double)info.Height
                            - (this.DrawingMargin * 2 + textBounds.Height + textSize)
                            - this.DrawingMargin * 4);
                int legendWidth = (int)(info.Width * 0.6 - this.DrawingMargin * 1.5);

                drawingPoint.X += info.Width * 0.4f - this.DrawingMargin * 0.5f;
                drawingPoint.Y += this.DrawingMargin * 4 + (float)textSize;

                LegendLabelSet[] offsetsPositions;
                var bmp=this.Legend.DrawLengends(legendWidth,legendHeight,out offsetsPositions);

                canvas.DrawBitmap(bmp, drawingPoint,paint);

                //凡例ラベルを描画

                float x = DrawingMargin;

                foreach (var set in offsetsPositions) {
                    string label = set.Label;
                    float y = set.Position + drawingPoint.Y + textBounds.Height * 0.4f;

                    canvas.DrawText(label,x,y,paint);
                 }
            }

            return surface;
        }


    }
}

