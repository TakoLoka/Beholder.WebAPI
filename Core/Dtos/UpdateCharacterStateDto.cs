using Core.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos
{
    public class UpdateCharacterStateDto: AbstractMessageBaseDto
    {
        public ICharacter UpdatedCharacter { get; set; }
    }
}
