﻿using System;
using System.Diagnostics;

namespace CASCLib
{
    public sealed class PerfCounter : IDisposable
    {
        private Stopwatch _sw;
        private readonly string _name;

        public PerfCounter(string name)
        {
            _name = name;
            _sw = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _sw.Stop();

            Console.WriteLine("{0} completed in {1}", _name, _sw.Elapsed);
            Logger.WriteLine("{0} completed in {1}", _name, _sw.Elapsed);
        }
    }
}
