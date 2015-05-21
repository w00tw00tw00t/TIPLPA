using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IronScheme;
using IronScheme.Runtime;
namespace plpaRobot
{
    class Schemer
    {
        public static void loadAllSchemeFiles(string directory)
        {
            Directory.GetFiles(directory).ToList().Where(f => f.EndsWith(".ss")).
                ToList().ForEach(f => loadSchemeFile(f));
        }

        public static void loadSchemeFile(string filename)
        {
            File.ReadAllText(filename).Eval();
        }

        public static object  Eval(string scheme, Object[] parameter)
        {
            if(parameter != null)
            {
               return scheme.Eval(parameter);
            }
            else
            {
               return scheme.Eval();
            }
        }

        public static object Eval(string scheme)
        {
            return Eval(scheme, null);
        }

        public static void resetEval()
        {
            "(interaction-environment (new-interaction-environment))".Eval();
        }
    }
}
