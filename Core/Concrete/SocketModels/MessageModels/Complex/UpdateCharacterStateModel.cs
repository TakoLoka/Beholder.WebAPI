using Core.Abstract;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Concrete.SocketModels.MessageModels.Complex
{
    public class UpdateCharacterStateModel: AbstractMessageModel
    {
        public override string Type { get { return MessageTypeConstants.UpdateCharacterState; } }
        public ICharacter UpdatedCharacter { get; set; }
    }
}
