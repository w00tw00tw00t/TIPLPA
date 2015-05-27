using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IronScheme;
using IronScheme.Runtime;
using System.Windows;
namespace plpaRobot
{
    public class Schemer
    {
        public static void loadAllSchemeFiles(string directory)
        {
            Directory.GetFiles(directory).ToList().Where(f => f.EndsWith(".ss")).
                ToList().ForEach(f => loadSchemeFile(f));
        }

        public static void loadSchemeFile(string filename)
        {
            try
            {
                File.ReadAllText(filename).Eval();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading file: " + e.Message);
            }

        }

        public static object Eval(string scheme, Object[] parameter = null)
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

        public static void doImports()
        {
            Eval("(import (ironscheme strings))");
            Eval("(import (srfi :6))");

            
        }

        public static string GetStringFromCommand(string command)
        {
            var eval = Eval(command);

            var s = eval as string;
            return s ?? ((Cons) eval).PrettyPrint;
        }

        public static List<Cons> ConvertNestedConsToList(Cons o)
        {
            var list = new List<Cons>();

            try {
            list.Add((Cons)o.car);
            
            if(o.cdr != null)
            {
               foreach( Cons d in ConvertNestedConsToList((Cons)o.cdr))
               {
                   list.Add(d);
               }
            }

            return list;
                } catch (Exception)
            {
                return list;
            }
        }

        internal static string GetFloorPlan()
        {
            try { 
                return (Eval("floorplan") as Cons).PrettyPrint;
            }
            catch (Exception )
            {
                return null;
            }

        }

        internal static Cons RunProgram(string p)
        {
            return (Cons) Schemer.Eval("(runProgram \""+p+"\")");
        }
    }
}
