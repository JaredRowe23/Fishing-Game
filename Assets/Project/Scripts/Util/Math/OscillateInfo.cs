namespace Fishing.Util.Math {
    public struct OscillateInfo {
        private float _value;
        public float Value { get => _value; set => _value = value; }
        private float _newTarget;
        public float NewTarget { get => _newTarget; set => _newTarget = value; }

        public OscillateInfo(float value, float newTarget) {
            _value = value;
            _newTarget = newTarget;
        }
    }
}