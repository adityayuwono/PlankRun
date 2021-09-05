using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class Player : Character
    {
        private Text _plankInformation;
        private string _plankInformationFormat;

        public Player(CharacterModel model, GameObject plankTemplate, Transform goal) : 
            base(model, plankTemplate, goal)
        {
            Control = new PlayerControl(model, this);

            _plankInformation = model.PlankInformation;
            if (_plankInformation != null)
            {
                _plankInformationFormat = _plankInformation.text;
            }
        }

        private int _plankCount;
        public override int PlankCount
        {
            get { return _plankCount; }
            protected set
            {
                _plankCount = value;

                if (_plankInformation != null)
                {
                    _plankInformation.text = string.Format(_plankInformationFormat, _plankCount);
                }
            }
        }
    }
}
