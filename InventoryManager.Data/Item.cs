﻿using System;
using System.Dynamic;
using System.Globalization;
using System.ComponentModel;

namespace InventoryManager.Data
{
    public class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
    }
}
