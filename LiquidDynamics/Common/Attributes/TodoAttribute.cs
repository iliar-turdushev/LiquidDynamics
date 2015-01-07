using System;

namespace Common.Attributes
{
   public sealed class TodoAttribute : Attribute
   {
      public TodoAttribute(string todo)
      {
         Check.NotNull(todo, "todo");
         Todo = todo;
      }

      public string Todo { get; private set; }
   }
}
