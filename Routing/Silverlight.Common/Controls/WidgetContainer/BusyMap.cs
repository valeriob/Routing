using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Silverlight.Common.Controls.WidgetContainer
{
    //  http://www.codeproject.com/Articles/23111/Making-a-Class-Schedule-Using-a-Genetic-Algorithm
    public class BusyMap
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        protected Control[,] Occupation { get; set; }


        public BusyMap(int x, int y)
        {
            X = x;
            Y = y;
            Occupation = new Control[x, y];
        }


        public Spot FindArea(int dx, int dy)
        {
            for (int j = 0; j <Y; j++)
                for (int i = 0; i <X; i++)
                    if (Is_Area_Available(i, j, dx, dy))
                        return new Spot { X = i, Y = j };
    
            return null;
        }

        protected bool Is_Area_Available(int i, int j, int dx, int dy)
        {
            for (int x = i; x < X; x++)
            {
                for (int y = j; y < Y; y++)
                {
                    if (Occupation[x,y] != null)
                        return false;
                    if (y - j == dy && x - i == dx)
                        return true;
                }
            }
            return true;
        }

        public void Set_Free(Control widget)
        {
            for (int i = 0; i <X; i++)
                for (int j = 0; j <Y; j++)
                    if (Occupation[i, j] == widget)
                        Occupation[i, j] = null;
        }

        public void Set_Busy(int x, int y, int dx, int dy, Control widget)
        {
            for (int i = x; i - x < dx && i <X; i++)
                for (int j = y; j - y < dy && j <Y; j++)
                    Occupation[i, j] = widget;
        }
    }

    public class AutoEnlarging_BusyMap
    {
        public int Original_X { get; protected set; }
        public int Original_Y { get; protected set; }

        public int Actual_X { get; protected set; }
        public int Actual_Y { get; protected set; }

        public bool AutoEnlargeX { get; set; }
        public bool AutoEnlargeY { get; set; }

        protected Control[][] Occupation { get; set; }


        public AutoEnlarging_BusyMap(int x, int y)
        {
            Actual_X = Original_X = x;
            Actual_Y = Original_Y = y;

            Occupation = Create_New(x, y);
        }


        public Spot FindArea(int dx, int dy)
        {
            for (int i = 0; i < Actual_X; i++)
                for (int j = 0; j < Actual_Y; j++)
                    if (Is_Area_Available(i, j, dx, dy))
                        return new Spot { X = i, Y = j };

            if (AutoEnlargeX)
            {
                int lastBusy = Find_Last_X_Busy();

                var newX = lastBusy + dx +1;
                var newOne = Clone_Bigger(Occupation, newX, Actual_Y);

                Set_NewOccupation(newOne, newX, Actual_Y);

                return new Spot { X = lastBusy + 1, Y = 0 };
            }

            if (AutoEnlargeY)
            {
                int lastBusy = Find_Last_Y_Busy();

                var newY = lastBusy + dy +1;
                var newOne = Clone_Bigger(Occupation, Actual_X, newY);

                Set_NewOccupation(newOne, Actual_X, newY);

                return new Spot { X = 0, Y = lastBusy + 1 };
            }

            return null;
        }

        public void Set_Free(Control control)
        {
            for (int i = 0; i < Actual_X; i++)
                for (int j = 0; j < Actual_Y; j++)
                    if (Occupation[i][j] == control)
                        Occupation[i][j] = null;
        }

        public void Set_Busy(int x, int y, int dx, int dy, Control control)
        {
            for (int i = x; i - x < dx && i < Actual_X; i++)
                for (int j = y; j - y < dy && j < Actual_Y; j++)
                    Occupation[i][j] = control;
        }



        protected Control[][] Create_New(int sizeX, int sizeY)
        {
            var control = new Control[sizeX][];
            for (int i = 0; i < sizeX; i++)
                control[i] = new Control[sizeY];
            return control;
        }

        protected Control[][] Clone_Bigger(Control[][] source, int sizeX, int sizeY)
        {
            if (sizeX < source.Length || sizeY < source[0].Length)
                throw new Exception("La destinazione deve essere uno spazio maggiore della sorgente");

            var bigger = Create_New(sizeX, sizeY);

            for (int i = 0; i < source.Length; i++)
                for (int j = 0; j < source[i].Length; j++)
                    bigger[i][j] = source[i][j];

            return bigger;
        }

        protected void Set_NewOccupation(Control[][] occupation, int sizeX, int sizeY)
        {
            Occupation = occupation;
            Actual_X = sizeX;
            Actual_Y = sizeY;
        }


        protected int Find_Last_X_Busy()
        {
            int j = Actual_X - 1;
            while (j >= -1 && Occupation[j][0] == null)
                j--;
            return j;
        }

        protected int Find_Last_Y_Busy()
        {
            int j = Actual_Y - 1;
            while (j >= -1 && Occupation[0][j] == null)
                j--;
            return j;
        }

        protected bool Is_Area_Available(int i, int j, int dx, int dy)
        {
            for (int x = i; x < Actual_X; x++)
            {
                for (int y = j; y < Actual_Y; y++)
                {
                    if (Occupation[x][y] != null)
                        return false;
                    if (y - j +1== dy && x - i +1== dx)
                        return true;
                }
            }
            return false;
        }
    }



    public class Spot
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class PageSpot: Spot
    {
        public Control Page { get; set; }


        public PageSpot(){}
        public PageSpot(Spot spot, Control page)
        {
            X = spot.X;
            Y = spot.Y;
            Page = page;
        }
    }


    public class PageBusyMap
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        protected Dictionary<Control, BusyMap> PageMaps { get; set; }


        public PageBusyMap(int x, int y)
        {
            X = x;
            Y = y;
            PageMaps = new Dictionary<Control, BusyMap>();
        }


        public PageSpot FindArea(int dx, int dy)
        {
            foreach (var page in PageMaps)
            {
                var spot = page.Value.FindArea(dx, dy);
                if (spot != null)
                    return new PageSpot(spot, page.Key);
            }
            return null;
        }

        public void Set_Free(Control widget)
        {
            foreach (var page in PageMaps)
                page.Value.Set_Free(widget);
        }

        public void Set_Busy(int x, int y, int dx, int dy, Control widget, Control page)
        {
            PageMaps[page].Set_Busy(x, y, dx, dy, widget);
        }

        public void Add_New_Page(Control page)
        {
            PageMaps.Add(page,new BusyMap(X, Y));
        }

    }

}
