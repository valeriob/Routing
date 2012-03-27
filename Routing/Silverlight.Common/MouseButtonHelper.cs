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

namespace Silverlight.Common
{
    public static class MouseButtonHelper
    {
        private const long k_DoubleClickSpeed = 500;
        private const double k_MaxMoveDistance = 20;

        private static long m_LastClickTicks = 0;
        private static Point m_LastPosition;
        private static object m_LastSender;

        public static bool IsDoubleClick(object sender, MouseButtonEventArgs e)
        {
            bool senderMatch = sender.Equals(m_LastSender);
            m_LastSender = sender;

            long clickTicks = DateTime.Now.Ticks;
            Point position = e.GetPosition(null);
            if (senderMatch)
            {
                long elapsedTicks = clickTicks - m_LastClickTicks;
                long elapsedTime = elapsedTicks / TimeSpan.TicksPerMillisecond;
                double distance = position.Distance(m_LastPosition);
                if (elapsedTime <= k_DoubleClickSpeed && distance <= k_MaxMoveDistance)
                {
                    // Double click!
                    m_LastClickTicks = 0;
                    return true;
                }
            }

            // Not a double click
            m_LastClickTicks = clickTicks;
            m_LastPosition = position;
            return false;
        }

        private static double Distance(this Point pointA, Point pointB)
        {
            double x = pointA.X - pointB.X;
            double y = pointA.Y - pointB.Y;
            return Math.Sqrt(x * x + y * y);
        }
    }
}
