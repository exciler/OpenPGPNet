
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using SmartCard.Runtime.Remoting.Channels.APDU;

namespace MyCompany.MyOnCardApp
{
    /// <summary>
    /// Summary description for MyServer.
    /// </summary>
    public class MyServer
    {
        /// <summary>
        /// specify the exposed remote object URI.
        /// </summary>
        private const string REMOTE_OBJECT_URI = "D27600012401";

        /// <summary>
        /// Register the server onto the card.
        /// </summary>
        /// <returns></returns>
        public static int Main()
        {
            // Register the channel the server will be listening to.
            ChannelServices.RegisterChannel(new APDUServerChannel());

            // Register this application as a server            
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(MyService), REMOTE_OBJECT_URI, WellKnownObjectMode.Singleton);

            return 0;
        }
    }
}

