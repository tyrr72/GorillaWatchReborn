using System;

namespace GorillaWatch.Models
{
    public class Mod
    {
        public bool isToggleable;
        public Action method;
        public string name;
        public bool enabled;
        public Action enable;
        public Action disable;

        public Mod(bool isToggleable, Action method, string name, bool enabled, Action enable, Action disable)
        {
            this.isToggleable = isToggleable;
            this.method = method;
            this.name = name;
            this.enabled = enabled;
            this.enable = enable;
            this.disable = disable;
        }
    }
}
