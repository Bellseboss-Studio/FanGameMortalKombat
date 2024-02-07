using System;

namespace Bellseboss.Pery.Scripts.Input
{
    public interface ICharacterV2
    {
        Action OnAction { get; set; }
    }
}