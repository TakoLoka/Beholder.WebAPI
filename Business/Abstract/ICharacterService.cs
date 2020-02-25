using Core.Entities.Models;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICharacterService
    {
        IDataResult<Character> GetCharacterById(string characterId);
        IDataResult<List<Character>> GetCharactersOfUser(string userId);
        IResult DeleteCharacter(string characterId);
        IDataResult<Character> UpdateCharacter(Character character);
        IResult CreateCharacter(Character character);
    }
}
