using System.Linq;
using Common;
using Mathematics.MathTypes;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.Kriging
{
   public sealed class KrigingMethodInterpolation
   {
      private readonly GridFunction _function;
      private readonly KrigingMethodParameters _parameters;

      public KrigingMethodInterpolation(
         GridFunction function,
         KrigingMethodParameters parameters)
      {
         Check.NotNull(function, "function");
         Check.NotNull(parameters, "parameters");
         
         _function = function;
         _parameters = parameters;
      }

      public double Interpolate(Point point)
      {
         var function = getNearestPoints(point);
         var krigingMethod = new KrigingMethod(function, _parameters.Variogram);
         return krigingMethod.GetValue(point);
      }

      private GridFunction getNearestPoints(Point point)
      {
         var nodesCount = _parameters.Nodes;
         var result = new DistanceInfo[nodesCount];

         for (var i = 0; i < nodesCount; i++)
         {
            result[i] = new DistanceInfo {Distance = double.MaxValue};
         }

         for (var i = 0; i < _function.N; i++)
         {
            var distanceInfo = new DistanceInfo
                                  {
                                     Point = _function.Grid(i),
                                     Value = _function[i],
                                     Distance = Point.Distance(_function.Grid(i), point)
                                  };

            for (var j = 0; j < nodesCount; j++)
            {
               if (distanceInfo.Distance < result[j].Distance)
               {
                  var temp = distanceInfo;

                  for (var k = j; k < nodesCount; k++)
                  {
                     var current = result[k];
                     result[k] = temp;
                     temp = current;
                  }

                  break;
               }
            }
         }

         return new GridFunction(result.Select(info => info.Point).ToArray(),
                                 result.Select(info => info.Value).ToArray());
      }
      
      private sealed class DistanceInfo
      {
         public Point Point { get; set; }
         public double Value { get; set; }
         public double Distance { get; set; }
      }
   }
}