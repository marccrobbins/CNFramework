using System;
using UnityEngine;

namespace CNFramework
{
   public class InputAction<T> : MonoBehaviour
   {
      public event Action<T> OnValueProcessed;
      
      public T value;
      public bool IsValid { get; protected set; }

      public virtual void ApplySettings()
      {
         
      }

      protected void ProcessValue(T value)
      {
         OnValueProcessed?.Invoke(value);
      }
   }
}
