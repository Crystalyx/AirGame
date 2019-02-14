﻿using System;
using System.Threading;
using GlLib.Client;
using GlLib.Client.Graphic;
using GlLib.Client.Input;
using GlLib.Common.Map;
using GlLib.Common.Registries;
using GlLib.Server;

namespace GlLib.Common
{
    internal static class Core
    {
        public static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
                string[] argsParts = arg.Split("=");
                (string variableName, string value) = (argsParts[0], argsParts[1]);
                Config.ProcessArgument(variableName, value);
            }

            if (Config._isIntegratedServer)
            {
                Thread serverThread = new Thread(() =>
                {
                    ServerInstance.StartServer();
                    ServerInstance.GameLoop();
                    ServerInstance.ExitGame();
                });

                ClientService._instance = new ClientService(Config._playerName, Config._playerPassword);
                Thread clientThread = new Thread(() =>
                {
                    ClientService._instance.StartClient();
                    ClientService._instance.GameLoop();
                    ClientService._instance.ExitGame();
                });
                serverThread.Start();
                clientThread.Start();
                //todo Main Menu
                while (ClientService._instance._state <= State.Starting && ServerInstance._state <= State.Starting)
                {
                }
                ClientService._instance.ConnectToIntegratedServer();
            }
        }
    }
}