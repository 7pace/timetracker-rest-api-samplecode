using System;
using System.IO;
using System.Text;

namespace _7pace.Timetracker.RestApiExample
{
    public static class DualOut
    {
        private static TextWriter _current;

        private class OutputWriter : TextWriter
        {
            private readonly string _fileName;

            public OutputWriter ( string fileName )
            {
                _fileName = fileName;
            }

            public override Encoding Encoding => _current.Encoding;

            public override void WriteLine ( string value )
            {
                _current.WriteLine( value );
                File.AppendAllLines( _fileName, new string[] { value } );
            }

            public override void Write ( string value )
            {
                _current.Write( value );
                File.AppendAllText( _fileName, value );
            }
        }

        public static void Init ( string fileName )
        {
            _current = Console.Out;
            Console.SetOut( new OutputWriter( fileName ) );
        }
    }
}