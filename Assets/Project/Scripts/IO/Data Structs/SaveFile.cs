namespace Fishing.IO {
    [System.Serializable]
    public class SaveFile {
        private string _name;
        public string Name { get => _name; private set => _name = value; }

        private float _money;
        public float Money { get => _money; private set => _money = value; }

        private string _dateTime;
        public string DateTime { get => _dateTime; private set => _dateTime = value; }

        private string _playtime;
        public string Playtime { get => _playtime; private set => _playtime = value; }

        private int _fishTypesCaught;
        public int FishTypesCaught { get => _fishTypesCaught; private set => _fishTypesCaught = value; }

        public SaveFile(string name, float money, string dateTime, string playtime, int fishTypesCaught) {
            Name = name;
            Money = money;
            DateTime = dateTime;
            Playtime = playtime;
            FishTypesCaught = fishTypesCaught;
        }
    }
}
