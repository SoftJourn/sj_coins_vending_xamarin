﻿namespace Softjourn.SJCoins.Core.API.Model
{
    public sealed class AccountOption
    {
        public string OptionName { get; set; }
        public string OptionIconName { get; set; }

        public AccountOption(string name, string iconName)
        {
            OptionName = name;
            OptionIconName = iconName;
        }
    }
}
