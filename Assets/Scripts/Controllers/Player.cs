using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class Player : Character
    {
        private int _addedPlanks;
        private Text _plankInformation;
        private string _plankInformationFormat;
        private float _secondsUntilInfoResets;

        public Player(CharacterModel model, GameObject plankTemplate, Transform goal) :
            base(model, plankTemplate, goal)
        {
            Control = new PlayerControl(model, this);

            _plankInformation = model.PlankInformation;
            _plankInformationFormat = _plankInformation.text;
        }

        private int _plankCount;
        public override int PlankCount
        {
            get { return _plankCount; }
            protected set
            {
                if (value>_plankCount)
                {
                    _addedPlanks++;
                    _secondsUntilInfoResets = 1f;
                    _plankInformation.enabled = true;
                    _plankInformation.text = string.Format(_plankInformationFormat, _addedPlanks);
                }

                _plankCount = value;
            }
        }

        public override void Update()
        {
            base.Update();

            _secondsUntilInfoResets -= Time.deltaTime;
            if (_secondsUntilInfoResets <= 0)
            {
                _addedPlanks = 0;
                _plankInformation.enabled = false;
            }
        }
    }
}
