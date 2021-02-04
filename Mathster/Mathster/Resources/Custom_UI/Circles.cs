﻿using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace Mathster.Resources.Custom_UI
{
    public class Circles
    {
        private readonly Func<SKImageInfo, SKPoint> centerFunc;
        public SKPoint Center { get; set; }
        public float Redius { get; set; }
        private SKCanvas canvas { get; set; }

        public Circles(float redius, Func<SKImageInfo, SKPoint> centerfunc)
        {
            centerFunc = centerfunc;
            Redius = redius;
        }

        public void DrawFullProgressBar(SKCanvasView view, string backgroundColorHex, string progressBarColorHex,
            float progressBarThickness, float progress, string progressColorHex)
        {
            // This not nice nothing or redundant code 
            view.PaintSurface += (sender, args) =>
            {
                canvas = args.Surface.Canvas;
                CalculateCenter(args.Info);
                canvas.Clear();
                DrawFullCircle(backgroundColorHex);
                DrawCircleBorder(progressBarThickness, progressBarColorHex);
                DrawProgress(progress, progressBarThickness, progressColorHex);
            };
        }

        public void DrawFullCircle(SKCanvasView view, string backgroundColorHex)
        {
            // This ugliness or redundant code 
            view.PaintSurface += (sender, args) =>
            {
                canvas = args.Surface.Canvas;
                CalculateCenter(args.Info);
                canvas.Clear();
                DrawFullCircle(backgroundColorHex);
            };
        }
        //TODO create a class for entry and colorHex --> All these parameters will be stored in array 
        public void DrawChart(SKCanvasView view, string backgroundColorHex, string progressBarColorHex,
            float progressBarThickness, float entry1, float entry2, float entry3, float entryMax, string entry1ColorHex,
            string entry2ColorHex, string entry3ColorHex)
        {
            view.PaintSurface += (sender, args) =>
            {
                float result1, result2, result3;
                ChartPartCalcualtion(entry1, entry2, entry3, entryMax, out result1, out result2,
                    out result3);
                canvas = args.Surface.Canvas;
                CalculateCenter(args.Info);
                canvas.Clear();
                DrawFullCircle(backgroundColorHex);
                DrawCircleBorder(progressBarThickness, progressBarColorHex);
                DrawProgress(result1, progressBarThickness, entry1ColorHex);
                DrawProgress(result2, progressBarThickness, entry2ColorHex);
                DrawProgress(result3, progressBarThickness, entry3ColorHex);
            };
        }

        /*
        private ChartPart[] ChartPartCalcualtion(ChartPart[] chartParts, float max)
        {
            ChartPart[] chartPartsCalculated = new ChartPart[chartParts.Length];
            float value = 0;
            for (int i = chartParts.Length; i > 0; i--)
            {
                value += chartParts[i].PartValue;
                chartPartsCalculated[i].PartValue = value / max * 100;
            }

            return chartPartsCalculated;
        }
         */
        
        private void ChartPartCalcualtion(float entry1, float entry2, float entry3, float max, out float result1,
            out float result2, out float result3)
        {
            result1 = (entry1 + entry2 + entry3) / max * 100;
            result2 = (entry2 + entry3) / max * 100;
            result3 = entry3 / max * 100;
        }

        public SKRect Rect => new SKRect(Center.X - Redius, Center.Y - Redius, Center.X + Redius, Center.Y + Redius);

        public void CalculateCenter(SKImageInfo argsInfo)
        {
            Center = centerFunc.Invoke(argsInfo);
        }

        private void DrawFullCircle(string backgroundColorHex)
        {
            canvas.DrawCircle(Center, Redius,
                new SKPaint()
                {
                    Style = SKPaintStyle.Fill,
                    Color = SKColor.Parse(backgroundColorHex)
                });
        }

        private void DrawCircleBorder(float progressBarThickness, string colorHex)
        {
            canvas.DrawCircle(Center, Redius,
                new SKPaint()
                {
                    StrokeWidth = progressBarThickness,
                    Color = SKColor.Parse(colorHex),
                    IsStroke = true
                });
        }

        private void DrawProgress(float progress, float progressBarThickness, string colorHex)
        {
            Func<float> step = () => progress;
            var angle = step.Invoke() * 3.6f;
            canvas.DrawArc(Rect, 270, angle, false,
                new SKPaint() {StrokeWidth = progressBarThickness, Color = SKColor.Parse(colorHex), IsStroke = true});
        }
    }
}