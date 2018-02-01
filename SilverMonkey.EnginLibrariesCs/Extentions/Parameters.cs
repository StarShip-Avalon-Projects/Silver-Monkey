using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Libraries.Extentions
{
    public static class Parameters
    {
        static public T[] GetParametersOfType<T>(this object[] t, params T[] args)
        {
            if (args != null && args.Length > 0)
            {
                return args.OfType<T>().ToArray();
            }
            return new T[0];
        }

        static public T GetParameter<T>(this object[] t, T[] args, int index = 0)
        {
            if (args != null && args.Length > index)
                return (T)args[index];
            return default(T);
        }
    }
}