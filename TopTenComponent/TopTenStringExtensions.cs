using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopTenComponent
{

    public static class TopTenStringExtensions
    {
    
        public static bool startsWithUppercase(this String str)
        { 
            return char.IsUpper(str[0]);            
        }
    }

}
