using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using InventoryManager.Data;
using Newtonsoft.Json;

namespace InventoryManager.Winforms.ViewModels
{
    public class WorldViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Filename { get; set; }

        public BindingList<Player> Players { get; set; }

        public BindingList<Item> Items { get; set; }

        private World world;

        public World World
        {
            set
            {
                if(world != value)
                {
                    world = value;
                    if(world != null)
                    {
                        Players = new BindingList<Player>(world.Players);
                        Items = new BindingList<Item>(world.Items);
                    }
                    else
                    {
                        Players = new BindingList<Player>(Array.Empty<Player>());
                        Items = new BindingList<Item>(Array.Empty<Item>());
                    }
                }    
            }
        }

        public WorldViewModel(World world = null)
        {
            World = world;
        }

        public void SaveWorld()
        {
            if (string.IsNullOrEmpty(Filename))
            {
                throw new InvalidProgramException("Filename expected.");
            }

            JsonSerializer serializer = new JsonSerializer()
            {
                Formatting = Formatting.Indented
            };
            using (StreamWriter streamWriter = new StreamWriter(Filename))
            using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(jsonWriter, world);
            }
        }

    }
}
