using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Avalonia.Collections;
using Avalonia.Data;
using Avalonia.LogicalTree;
using Avalonia.Metadata;

namespace Avalonia.Controls
{
    public partial class NativeMenu : AvaloniaObject, IEnumerable<NativeMenuItem>
    {
        private AvaloniaList<NativeMenuItem> _items =
            new AvaloniaList<NativeMenuItem> { ResetBehavior = ResetBehavior.Remove };
        private NativeMenuItem _parent;
        [Content]
        public IList<NativeMenuItem> Items => _items;

        public NativeMenu()
        {
            _items.Validate = Validator;
            _items.CollectionChanged += ItemsChanged;
        }

        private void Validator(NativeMenuItem obj)
        {
            if (obj.Parent != null)
                throw new InvalidOperationException("NativeMenuItem already has a parent");
        }

        private void ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.OldItems!=null)
                foreach (NativeMenuItem i in e.OldItems)
                    i.Parent = null;
            if(e.NewItems!=null)
                foreach (NativeMenuItem i in e.NewItems)
                    i.Parent = this;
        }

        public static readonly DirectProperty<NativeMenu, NativeMenuItem> ParentProperty =
            AvaloniaProperty.RegisterDirect<NativeMenu, NativeMenuItem>("Parent", o => o.Parent, (o, v) => o.Parent = v);

        public NativeMenuItem Parent
        {
            get => _parent;
            set => SetAndRaise(ParentProperty, ref _parent, value);
        }

        
        public void Add(NativeMenuItem item) => _items.Add(item);
        
        public IEnumerator<NativeMenuItem> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
