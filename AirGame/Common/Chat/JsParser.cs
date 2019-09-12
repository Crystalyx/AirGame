using System;
using GlLib.Client.Api;
using GlLib.Common.Io;
using GlLib.Utils.StringParser;
using Jint;

namespace GlLib.Common.Chat
{
    public class JsParser : IParser
    {
        public Engine jsEngine;
        public bool isLogSetUp = false;

        public JsParser()
        {
            jsEngine = new Engine(_cfg => _cfg.AllowClr());
            SetupEngine(jsEngine);
        }

        private void SetupEngine(Engine _engine)
        {
            _engine.SetValue("whoami", new Action<Action<string>>(_io=> _io(Proxy.GetClient().player.nickname)));
            
//            _engine.SetValue("where", new Action<string, object>(SidedConsole.WriteLine(Proxy.GetServer().playerInfo[_name].Position)));
        }

        public void Parse(string _arg, IStringIo _io)
        {
            if(!isLogSetUp)
                jsEngine.SetValue("log", new Action<string>(_io.Output));
            try
            {
                jsEngine.Execute(_arg);
            }
            catch (Exception e)
            {
                _io.Output(e.ToString());
            }
        }
    }
}