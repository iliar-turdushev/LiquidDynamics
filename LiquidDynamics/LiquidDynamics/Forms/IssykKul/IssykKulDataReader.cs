using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Mathematics.MathTypes;
using GraphPoint3D = ControlLibrary.Types.Point3D;

namespace LiquidDynamics.Forms.IssykKul
{
   internal static class IssykKulDataReader
   {
      private const char DataDelimiter = '\t';
      private const char GraphDataDelimiter = ' ';

      private const string Border = "C_BROVKA";
      private const string Point = "C_PK";

      internal static IssykKulGraphData ReadGraphData(byte[] bytes)
      {
         var borders = new List<GraphPoint3D[]>();
         var points = new List<GraphPoint3D>();

         using (var stream = new StreamReader(new MemoryStream(bytes)))
         {
            string line;
            List<GraphPoint3D> border = null;

            while ((line = stream.ReadLine()) != null)
            {
               if (string.IsNullOrWhiteSpace(line))
               {
                  if (border != null)
                  {
                     borders.Add(border.ToArray());
                     border = null;
                  }

                  continue;
               }

               var data = line.Split(new[] {GraphDataDelimiter}, StringSplitOptions.RemoveEmptyEntries);
               var pointType = data[1];
               var x = float.Parse(data[2], CultureInfo.InvariantCulture);
               var y = float.Parse(data[3], CultureInfo.InvariantCulture);
               var z = float.Parse(data[4], CultureInfo.InvariantCulture);
               var graphPoint = new GraphPoint3D(x, y, -z);
               
               switch (pointType)
               {
                  case Border:
                     if (border == null)
                     {
                        border = new List<GraphPoint3D> {graphPoint};
                     }
                     else
                     {
                        border.Add(graphPoint);
                     }
                     break;

                  case Point:
                     points.Add(graphPoint);
                     break;
               }
            }
         }

         return new IssykKulGraphData(borders.ToArray(), points.ToArray());
      }

      internal static Point3D[] ReadData(byte[] bytes)
      {
         var points = new List<Point3D>();

         using (var stream = new StreamReader(new MemoryStream(bytes)))
         {
            string line;

            while ((line = stream.ReadLine()) != null)
            {
               var data = line.Split(new[] {DataDelimiter}, StringSplitOptions.RemoveEmptyEntries);
               var x = double.Parse(data[0], CultureInfo.InvariantCulture);
               var y = double.Parse(data[1], CultureInfo.InvariantCulture);
               var z = double.Parse(data[2], CultureInfo.InvariantCulture);
               points.Add(new Point3D(x, y, -z));
            }
         }

         return points.ToArray();
      }
   }
}