using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Controllers
{
    public class DisabledControl : IControlCharacter
    {
        public void BoostedJump()
        {
        }

        public void Jump(float multiplier = 1)
        {
        }

        public void Move()
        {
        }

        public void Rotate(float signedDirection)
        {
        }
    }
}
