using Core.Abstract;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Concrete.SocketModels.MessageModels.Basic
{
    public class AbandonRoomModel: AbstractMessageModel
    {
        public override string Type { get { return MessageTypeConstants.AbandonRoom; } }
    }
}
