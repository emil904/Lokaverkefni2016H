using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace LokaverkefniServer
{
    class Program
    {
        

        static void Main(string[] args)
        {
            dbClass db = new dbClass(); //býr til db object
            db.TengingVidDB(); //Keyrir function inní dbClass.cs sem tengir við tsuts.tskoli.is db'ið
            
            
        }

        
    }


}
