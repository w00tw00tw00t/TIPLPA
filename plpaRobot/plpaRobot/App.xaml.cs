﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using IronScheme;
using IronScheme.Runtime;

namespace plpaRobot
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
      

        public void lala ()
        {
            Callable load = "load".Eval<Callable>();

        }
        
    }
}
