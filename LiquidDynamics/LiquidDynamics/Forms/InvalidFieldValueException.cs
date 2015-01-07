using System;

namespace LiquidDynamics.Forms
{
   internal sealed class InvalidFieldValueException : Exception
   {
      public InvalidFieldValueException(string message) : base(message)
      {
      }
   }
}