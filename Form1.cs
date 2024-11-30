

using DevExpress.XtraCharts.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawingProject
{
    public partial class Drwaing : Form
    {
        int MosueMoveX, MouseMoveY,
             MouseDownX, MouseDownY;
        Point point;

        struct stDraw
        {
            public bool startPaint;
            public Bitmap bitmap;
            public Graphics graphic;
            public Point CurrdenatPointX, CurrdenatPointY;
            public Pen pen;
            public Color PenColor;
            public Rectangle rectangle;
        }
        stDraw Draw; 
        enum enPaintTools 
        {  
            Fill,
            Pen, 
            Eraser, 
            Line,
            Rectangle, 
            Traingale,
            Circle,
            NoTool
        }
        enPaintTools PaintTool;
       
        void ShowButtonsColorsMenu()
        {
            button1.BackColor = Color.Black;
            button2.BackColor = Color.Brown;
            button3.BackColor = Color.DarkCyan;
            button4.BackColor = Color.Yellow;
            button5.BackColor = Color.Chocolate;
            button6.BackColor = Color.DarkBlue;
            button7.BackColor = Color.Blue;
            button8.BackColor = Color.Green;
            button9.BackColor = Color.DarkGoldenrod;
            button10.BackColor = Color.Violet;
            button11.BackColor = Color.MediumPurple;
            button12.BackColor = Color.White;
            button12.Enabled = false;
        }
        void CreatWhiteImage()
        {
            Draw.bitmap = new Bitmap(pic_BordPaint.Width, pic_BordPaint.Height);
            Draw.graphic = Graphics.FromImage(Draw.bitmap);
            Draw.graphic.Clear(Color.White);
            pic_BordPaint.Image = Draw.bitmap;


        }
        void ChangeMouseCurseIcon(string ImageName)
        {
            Bitmap btm = new Bitmap(new Bitmap(@ImageName),19, 19);
            pic_BordPaint.Cursor = new Cursor(btm.GetHicon());
        }
        void ShowPanttesColorsDialog()
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();
            btn_ShowColor.BackColor = colorDialog.Color;
            Draw.PenColor = colorDialog.Color;
        }
        

        Point GetMouseDownLocation(Point MouseLocation)
        {
            Draw.CurrdenatPointY = MouseLocation; 
            return MouseLocation; 
        }
        Point GetMouseMoveLocation(Point MouseMoveLocation)
        {
            Draw.CurrdenatPointX = MouseMoveLocation;
            return MouseMoveLocation;
        }

        enPaintTools enGetPaintTool(byte PaintTool)
        {
            enPaintTools Tool = (enPaintTools)PaintTool;
            switch (Tool)
            {
                case enPaintTools.Fill:
                  return enPaintTools.Fill;
                break;
                case enPaintTools.Pen:
                  return enPaintTools.Pen;
                break;
                case enPaintTools.Eraser:
                    return enPaintTools.Eraser; 
                break;
                case enPaintTools.Line:
                    return enPaintTools.Line; 
                break;
                case enPaintTools.Rectangle:
                    return enPaintTools.Rectangle; 
                break;
                case enPaintTools.Traingale:
                    return enPaintTools.Traingale; 
                break;
                case enPaintTools.Circle:
                    return enPaintTools.Circle;
                break;
            }
            return enPaintTools.NoTool; 
        }


        void PaintByPen(Color PenColor , short Width)
        {

            Draw.pen = new Pen(PenColor, Width);
            Draw.graphic.DrawLine(Draw.pen, Draw.CurrdenatPointX, Draw.CurrdenatPointY);
            Draw.CurrdenatPointY = Draw.CurrdenatPointX;
           

        }
        void DrawRectangle()
        {

            Draw.pen = new Pen(Draw.PenColor, 1);
            Draw.graphic.Clear(Color.White);
            Draw.graphic.DrawRectangle(Draw.pen, MouseDownX, MouseDownY, MosueMoveX-MouseDownX, MouseMoveY-MouseDownY);
           

        }
        void DrawCircle()
        {
            Draw.pen = new Pen(Draw.PenColor, 1);
            Draw.graphic.Clear(Color.White);
            Draw.graphic.DrawEllipse(Draw.pen, MouseDownX, MouseDownY, MosueMoveX - MouseDownX, MouseMoveY - MouseDownY);
            
        }
        void DrawLine()
        {
            Draw.pen = new Pen(Draw.PenColor, 1);
            Draw.graphic.Clear(Color.White);
            Draw.graphic.DrawLine(Draw.pen, MouseDownX, MouseDownY, MosueMoveX , MouseMoveY );

        }

        private void chckpixel(Bitmap bm, Stack<Point> sp, int x, int y, Color old_color, Color new_color)
        {
           
            Color cl = bm.GetPixel(x, y);
            if (cl == old_color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }

        }
        public void fill(Bitmap bm, int x, int y, Color new_col)
        {
            Color old_color = bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
           
            while (pixel.Count > 0)
            {
                
                Point pt = (Point)pixel.Pop();
                if (pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
              
                {
                    chckpixel(bm, pixel, pt.X - 1, pt.Y, old_color, new_col);
                    chckpixel(bm, pixel, pt.X, pt.Y - 1, old_color, new_col);
                    chckpixel(bm, pixel, pt.X + 1, pt.Y, old_color, new_col);
                    chckpixel(bm, pixel, pt.X, pt.Y + 1, old_color, new_col);
                }
            }
        }
           
          void StartDraw(bool IsPaintStart = false, enPaintTools Tool = enPaintTools.NoTool)
          { 

            if (IsPaintStart == true)
            {
                switch (PaintTool)
                {
                    case enPaintTools.Pen:
                        PaintByPen(Draw.PenColor, 1);
                        break;
                    case enPaintTools.Eraser:
                        PaintByPen(Color.White, 40);
                        break;
                    case enPaintTools.Rectangle:
                        DrawRectangle();
                        break;
                    case enPaintTools.Circle:
                        DrawCircle();
                        break;
                    case enPaintTools.Line:
                        DrawLine();
                    break;
                    
                }
            }

         }
      
        public Drwaing()
        {
            InitializeComponent();
            ShowButtonsColorsMenu();
            CreatWhiteImage();


        }
       
        private void pic_BordPaint_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownX = e.X;
            MouseDownY = e.Y;
            GetMouseDownLocation(e.Location);
        }

        private void pic_BordPaint_MouseUp(object sender, MouseEventArgs e)
        {
            StartDraw(false, enPaintTools.NoTool);
        }

        private void pic_BordPaint_MouseMove(object sender, MouseEventArgs e)
        {
            MosueMoveX = e.X;
            MouseMoveY = e.Y;
            GetMouseMoveLocation(e.Location);
            StartDraw(Draw.startPaint, PaintTool);
            pic_BordPaint.Refresh();
        }


        private void pic_BordPaint_MouseClick(object sender, MouseEventArgs e)
        {
            if(PaintTool == enPaintTools.Fill)
            {
                point = e.Location;
                fill(Draw.bitmap, point.X, point.Y,Draw.PenColor);
            }
            Draw.startPaint = !Draw.startPaint;
           
           
        }

        private void btn_PaintTools_Click(object sender, EventArgs e)
        {
           
            PaintTool = enGetPaintTool(Convert.ToByte(((Button)sender).Tag));

            if (Convert.ToByte(((Button)sender).Tag) == 0 )
            ChangeMouseCurseIcon(@"C:\Users\chmik\Desktop\drawingProjectImg\PaintImages\bucket_paint_icon.png");
            if (Convert.ToByte(((Button)sender).Tag) == 1)
            ChangeMouseCurseIcon(@"C:\Users\chmik\Desktop\drawingProjectImg\PaintImages\pencil.png");
            if (Convert.ToByte(((Button)sender).Tag) == 2)
            ChangeMouseCurseIcon(@"C:\Users\chmik\Desktop\drawingProjectImg\PaintImages\eraser.png");
            if (Convert.ToByte(((Button)sender).Tag) == 3)
            ChangeMouseCurseIcon(@"C:\Users\chmik\Desktop\drawingProjectImg\PaintImages\Line.png");
            if (Convert.ToByte(((Button)sender).Tag) == 4)
            ChangeMouseCurseIcon(@"C:\Users\chmik\Desktop\drawingProjectImg\PaintImages\rectangle.png");
            if (Convert.ToByte(((Button)sender).Tag) == 5)
            ChangeMouseCurseIcon(@"C:\Users\chmik\Desktop\drawingProjectImg\PaintImages\Triangle.png");
            if (Convert.ToByte(((Button)sender).Tag) == 6)
            ChangeMouseCurseIcon(@"C:\Users\chmik\Desktop\drawingProjectImg\PaintImages\circle.png");
        }

        private void btn_Panttes_Click(object sender, EventArgs e)
        {
            ShowPanttesColorsDialog();

        }

        private void btnMenuColors_Click(object sender, EventArgs e)
        {
            btn_ShowColor.BackColor = ((Button)sender).BackColor;
            Draw.PenColor = btn_ShowColor.BackColor;
        }

       

       
    }
}
