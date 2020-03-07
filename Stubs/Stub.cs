using System;
using System.Diagnostics;
using System.Text;
internal static class Stub
{
    private static StringBuilder sSb =new StringBuilder();

    public static void Log( Type type, string name, params object[] args )
    {
        sSb.Clear();
        sSb.Append( $"[STUB] {type.FullName}.{name}(" );

        bool first = true;
        foreach ( var item in args )
        {
            if ( !first )
                sSb.Append( ", " );

            sSb.Append( item );
        }

        sSb.Append( ")" );

        Debug.WriteLine( sSb.ToString() );
    }
}
