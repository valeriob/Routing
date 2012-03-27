using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;

namespace Silverlight.Common.Helpers
{

    public class Memento<T>
    {
        private Dictionary<PropertyInfo, object> storedProperties = new Dictionary<PropertyInfo, object>();

        public Memento(T originator)
        {
            this.InitializeMemento(originator);
        }

        public T Originator
        {
            get;
            protected set;
        }

        public void Restore(T originator)
        {
            foreach (var pair in this.storedProperties)
            {
                pair.Key.SetValue(originator, pair.Value, null);
            }
        }

        private void InitializeMemento(T originator)
        {
            if (originator == null)
                throw new ArgumentNullException("Originator", "Originator cannot be null");

            this.Originator = originator;
            IEnumerable<PropertyInfo> propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(p => p.CanRead && p.CanWrite);

            foreach (PropertyInfo property in propertyInfos)
                this.storedProperties[property] = property.GetValue(originator, null);
        }
    }

    public class Caretaker<T> : IEditableObject
    {
        private Memento<T> memento;
        private T target;

        public T Target
        {
            get
            {
                return this.target;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Target", "Target cannot be null");
                }

                if (Object.ReferenceEquals(this.Target, value))
                    return;

                this.target = value;
            }
        }

        public Caretaker(T target)
        {
            this.Target = target;
        }

        public void BeginEdit()
        {
            if (this.memento == null)
                this.memento = new Memento<T>(this.Target);
        }

        public void CancelEdit()
        {
            if (this.memento == null)
                throw new ArgumentNullException("Memento", "BeginEdit() is not invoked");

            this.memento.Restore(Target);
            this.memento = null;
        }

        public void EndEdit()
        {
            if (this.memento == null)
                throw new ArgumentNullException("Memento", "BeginEdit() is not invoked");

            this.memento = null;
        }

    }

    /*
     
    private void OnEditPersonCommandExecute()
    {
        Caretaker<Person> editableObject = new Caretaker<Person>( this.SelectedPerson );
        editableObject.BeginEdit();
 
        this.ModalDialogWorker.ShowDialog<Person>(
            this.ModalDialog, this.EditPersonControl, this.SelectedPerson, p =>
            {
                if ( this.ModalDialog.DialogResult.HasValue &&
                    this.ModalDialog.DialogResult.Value )
                {
                    editableObject.EndEdit();
                }
                else
                {
                    editableObject.CancelEdit();
                }
            } );
    } 
     * 
     */
}
