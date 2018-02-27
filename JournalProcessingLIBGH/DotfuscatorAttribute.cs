//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JournalProcessingLIBGH
//{
//    class DotfuscatorAttribute
//    {
//    }
//}

using System;
using System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly), ComVisible(false)]
public sealed class DotfuscatorAttribute : Attribute
{
    private string a;

    private bool b;

    private int c;

    public string A
    {
        get
        {
            return this.a;
        }
    }

    public bool B
    {
        get
        {
            return this.b;
        }
    }

    public int C
    {
        get
        {
            return this.c;
        }
    }

    public DotfuscatorAttribute(string a, int c, bool b)
    {
        this.a = a;
        this.c = c;
        this.b = b;
    }
}
